using BankingSystem.Core.Entities;
using BankingSystem.infrastructure.Data;
using BankingSystem.infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDBContext _context;

        public AccountRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        // Generate unique Account Number
        public string GenerateAccountNumber()
        {
            var random = new Random();
            var timestamp = DateTime.UtcNow.Ticks.ToString().Substring(5, 10); // Use a substring to ensure it's 10 digits

            var randomDigits = random.Next(100000, 999999).ToString();

            var accountNumber = timestamp + randomDigits;

            return accountNumber;
        }

        // Check if account exists by Email
        public async Task<bool> AccountExistsByEmail(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        // Check if account exists by Mobile
        public async Task<bool> AccountExistsByMobile(string mobile)
        {
            return await _context.Accounts.AnyAsync(a => a.Mobile == mobile);
        }

        // Create Account
        public async Task<Account> CreateAccount(Account account)
        {
            account.AccountNumber = GenerateAccountNumber();
            account.Balance = 0; // Set default balance
            account.DateCreated = DateTime.UtcNow;

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        // Get Account by Account Number
        public async Task<Account> GetAccount(string accountNumber)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
        }

        // Get All Accounts
        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // Update Account
        public async Task UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
    }

}
