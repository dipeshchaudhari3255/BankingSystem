

using BankingSystem.Core.Entities;
using BankingSystem.infrastructure.Data;
using BankingSystem.infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDBContext _context;

        public TransactionRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<List<Transaction>> GetTransactionsByAccountNumber(string accountNumber)
        {
            return await _context.Transactions
                .Where(t => t.AccountNumber == accountNumber)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _context.Transactions
                .OrderBy(t => t.TransactionDate)
                .ToListAsync();
        }

    }

}
