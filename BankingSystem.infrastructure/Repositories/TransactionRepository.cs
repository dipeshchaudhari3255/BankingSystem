using BankingSystem.Core.Entities;
using BankingSystem.infrastructure.Data;
using BankingSystem.infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.infrastructure.Repositories
{
    public class TransactionRepository(ApplicationDBContext _context) : ITransactionRepository
    {

        // Create Transaction with transaction support
        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
                return transaction;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        // Get Transactions by Account Number with optimization
        public async Task<List<Transaction>> GetTransactionsByAccountNumber(string accountNumber)
        {
            return await _context.Transactions
                .Where(t => t.AccountNumber == accountNumber)
                .OrderByDescending(t => t.TransactionDate) // Latest transactions first
                .AsNoTracking()
                .ToListAsync();
        }

        // Get All Transactions with optimized ordering
        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _context.Transactions
                .AsNoTracking()
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
    }
}
