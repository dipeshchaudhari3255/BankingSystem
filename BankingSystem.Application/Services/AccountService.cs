using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using BankingSystem.Core.Entities;
using BankingSystem.infrastructure.Interfaces;

namespace BankingSystem.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        // Create Account
        public async Task<AccountDto> CreateAccount(AccountDto accountDto)
        {
            if (await _accountRepository.AccountExistsByEmail(accountDto.Email))
            {
                throw new Exception("An account with this email already exists.");
            }

            if (await _accountRepository.AccountExistsByMobile(accountDto.Mobile))
            {
                throw new Exception("An account with this mobile number already exists.");
            }

            var account = _mapper.Map<Account>(accountDto);

            var createdAccount = await _accountRepository.CreateAccount(account);
            return _mapper.Map<AccountDto>(createdAccount);
        }

        // Get Account by Account Number
        public async Task<AccountDto> GetAccount(string accountNumber)
        {
            var account = await _accountRepository.GetAccount(accountNumber);
            if (account == null)
            {
                return null;
            }

            return _mapper.Map<AccountDto>(account);
        }

        // Get All Accounts
        public async Task<IEnumerable<AccountDto>> GetAccounts()
        {
            var accounts = await _accountRepository.GetAccounts();
            return accounts.Select(account => new AccountDto
            {
                AccountNumber = account.AccountNumber,
                Name = account.Name,
                Email = account.Email,
                Mobile = account.Mobile,
                Balance = account.Balance,
                DateCreated = account.DateCreated
            }).ToList();
        }

        public async Task UpdateAccount(Account account)
        {
            await _accountRepository.UpdateAccount(account);
        }
    }
}
