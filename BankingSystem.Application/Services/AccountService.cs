using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;
using BankingSystem.infrastructure.Interfaces;

namespace BankingSystem.Application.Services
{
    public class AccountService(IAccountRepository _accountRepository, IMapper _mapper, IEmailService _emailService) : IAccountService
    {
        // Create Account
        public async Task<AccountDto> CreateAccount(AccountDto accountDto)
        {
            // Check if email or mobile already exists in a single query
            if (await _accountRepository.AccountExists(accountDto.Email, accountDto.Mobile))
            {
                throw new InvalidOperationException("An account with this email or mobile number already exists.");
            }

            var account = _mapper.Map<Account>(accountDto);
            var createdAccount = await _accountRepository.CreateAccount(account);

            // Send email notification only if enabled
            if (await _emailService.IsEmailNotificationEnabled(account.AccountNumber))
                await _emailService.SendEmail(MailType.AccountCreation, account);

            return _mapper.Map<AccountDto>(createdAccount);
        }

        // Get Account by Account Number
        public async Task<AccountDto?> GetAccount(string accountNumber)
        {
            var account = await _accountRepository.GetAccount(accountNumber);
            return account is null ? null : _mapper.Map<AccountDto>(account);
        }

        // Get All Accounts
        public async Task<IEnumerable<AccountDto>> GetAccounts()
        {
            var accounts = await _accountRepository.GetAccounts();
            return _mapper.Map<List<AccountDto>>(accounts);
        }

        // Update Account
        public async Task UpdateAccount(Account account)
        {
            if (account is null)
                throw new ArgumentNullException(nameof(account), "Account cannot be null.");

            await _accountRepository.UpdateAccount(account);
        }
    }
}
