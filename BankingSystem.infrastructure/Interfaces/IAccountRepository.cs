using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Entities;

namespace BankingSystem.infrastructure.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> AccountExistsByEmail(string email);
        Task<bool> AccountExistsByMobile(string mobile);
        Task<Account> CreateAccount(Account account);
        Task<Account> GetAccount(string accountNumber);
        Task<IEnumerable<Account>> GetAccounts();
        Task UpdateAccount(Account account);
    }

}
