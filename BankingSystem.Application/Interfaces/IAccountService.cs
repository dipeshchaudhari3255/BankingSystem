using BankingSystem.Application.DTOs;

namespace BankingSystem.Application.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAccount(AccountDto account);
        Task<AccountDto?> GetAccount(string accountNumber);
        Task<IEnumerable<AccountDto>> GetAccounts();
    }

}
