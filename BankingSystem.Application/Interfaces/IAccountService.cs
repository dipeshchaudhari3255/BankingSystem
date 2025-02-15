using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Application.DTOs;

namespace BankingSystem.Application.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAccount(AccountDto account);
        Task<AccountDto> GetAccount(string accountNumber);
        Task<IEnumerable<AccountDto>> GetAccounts();
    }

}
