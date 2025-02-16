using BankingSystem.Core.Common;
using BankingSystem.Core.Entities;
using BankingSystem.infrastructure.Data;
using BankingSystem.infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.infrastructure.Repositories
{
    public class AccountRepository(ApplicationDBContext _context) : IAccountRepository
    {
        // Check if account exists by Email or Mobile
        public async Task<bool> AccountExists(string email, string mobile)
        {
            return await _context.Accounts
                .AsNoTracking()
                .AnyAsync(a => a.Email == email || a.Mobile == mobile);
        }

        // Create Account with transaction support
        public async Task<Account> CreateAccount(Account account)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                account.AccountNumber = Helper.GenerateAccountNumber();
                account.DateCreated = DateTime.UtcNow;

                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return account;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Get Account by Account Number with optimized query
        public async Task<Account> GetAccount(string accountNumber)
        {
            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

            if (account == null)
                throw new Microsoft.VisualStudio.Services.Account.AccountNotFoundException("Account not found.");

            return account;
        }

        // Get All Accounts
        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _context.Accounts
                .AsNoTracking()
                .ToListAsync();
        }

        // Update Account with transaction support
        public async Task UpdateAccount(Account account)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
