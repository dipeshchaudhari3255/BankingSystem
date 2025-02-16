using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;
using BankingSystem.infrastructure.Interfaces;

namespace BankingSystem.Application.Services
{
    public class TransactionService(
        IEmailService _emailService,
        ITransactionRepository _transactionRepository,
        IAccountRepository _accountRepository,
        IMapper _mapper) : ITransactionService
    {
        // Deposit Funds
        public async Task<TransactionDto> Deposit(TransactionDto transactionDto)
        {
            var account = await _accountRepository.GetAccount(transactionDto.AccountNumber)
                          ?? throw new Microsoft.VisualStudio.Services.Account.AccountNotFoundException("Account not found.");

            account.Balance += transactionDto.Amount;

            var transaction = new Transaction
            {
                TransactionID = Guid.NewGuid().ToString(),
                AccountNumber = account.AccountNumber,
                Amount = transactionDto.Amount,
                TransactionType = TransactionType.Credit,
                BalanceAfterTransaction = account.Balance,
                TransactionDate = DateTime.UtcNow
            };

            await SaveTransactionAndUpdateAccount(transaction, account);
            return _mapper.Map<TransactionDto>(transaction);
        }

        // Withdraw Funds
        public async Task<TransactionDto> Withdraw(TransactionDto transactionDto)
        {
            var account = await _accountRepository.GetAccount(transactionDto.AccountNumber)
                          ?? throw new Microsoft.VisualStudio.Services.Account.AccountNotFoundException("Account not found.");

            if (account.Balance < transactionDto.Amount)
                throw new Exception("Insufficient balance.");

            account.Balance -= transactionDto.Amount;

            var transaction = new Transaction
            {
                TransactionID = Guid.NewGuid().ToString(),
                AccountNumber = account.AccountNumber,
                Amount = transactionDto.Amount,
                TransactionType = TransactionType.Debit,
                BalanceAfterTransaction = account.Balance,
                TransactionDate = DateTime.UtcNow
            };

            await SaveTransactionAndUpdateAccount(transaction, account);
            return _mapper.Map<TransactionDto>(transaction);
        }

        // Transfer Money
        public async Task<TransactionDto> Transfer(TransferDto transferDto)
        {
            var senderAccount = await _accountRepository.GetAccount(transferDto.SenderAccountNumber)
                               ?? throw new Microsoft.VisualStudio.Services.Account.AccountNotFoundException("Sender account not found.");
            var receiverAccount = await _accountRepository.GetAccount(transferDto.ReceiverAccountNumber)
                                 ?? throw new Microsoft.VisualStudio.Services.Account.AccountNotFoundException("Receiver account not found.");

            if (senderAccount.Balance < transferDto.Amount)
                throw new Exception("Insufficient balance for transfer.");

            // Process debit transaction for sender
            senderAccount.Balance -= transferDto.Amount;
            var senderTransaction = CreateTransaction(senderAccount, transferDto.Amount, TransactionType.Debit);

            // Process credit transaction for receiver
            receiverAccount.Balance += transferDto.Amount;
            var receiverTransaction = CreateTransaction(receiverAccount, transferDto.Amount, TransactionType.Credit);

            await SaveTransactionAndUpdateAccount(senderTransaction, senderAccount);
            await SaveTransactionAndUpdateAccount(receiverTransaction, receiverAccount);

            return _mapper.Map<TransactionDto>(senderTransaction);
        }

        // Get Transactions By Account Number
        public async Task<List<Transaction>> GetTransactionsByAccountNumber(string accountNumber)
        {
            var transactions = await _transactionRepository.GetTransactionsByAccountNumber(accountNumber);
            return _mapper.Map<List<Transaction>>(transactions);
        }

        // Get Daily Transaction Summary
        public async Task<List<DailyTransactionSummaryDto>> GetDailyTransactionSummary()
        {
            var transactions = await _transactionRepository.GetAllTransactions();

            return transactions
                .Where(t => t.TransactionDate.Date == DateTime.UtcNow.Date) // Filter today's transactions early
                .GroupBy(t => t.TransactionDate.Date)
                .Select(g => new DailyTransactionSummaryDto
                {
                    Date = g.Key,
                    TotalDeposits = g.Where(t => t.TransactionType == TransactionType.Credit).Sum(t => t.Amount),
                    TotalWithdrawals = g.Where(t => t.TransactionType == TransactionType.Debit).Sum(t => t.Amount),
                    TotalBalance = g.OrderByDescending(t => t.TransactionDate).FirstOrDefault()?.BalanceAfterTransaction ?? 0
                })
                .OrderBy(d => d.Date)
                .ToList();
        }

        // Helper Method to Create Transaction
        private static Transaction CreateTransaction(Account account, decimal amount, TransactionType type)
        {
            return new Transaction
            {
                TransactionID = Guid.NewGuid().ToString(),
                AccountNumber = account.AccountNumber,
                Amount = amount,
                TransactionType = type,
                BalanceAfterTransaction = account.Balance,
                TransactionDate = DateTime.UtcNow
            };
        }

        // Helper Method to Save Transaction and Update Account
        private async Task SaveTransactionAndUpdateAccount(Transaction transaction, Account account)
        {
            await _transactionRepository.CreateTransaction(transaction);
            await _accountRepository.UpdateAccount(account);

            if (await _emailService.IsEmailNotificationEnabled(account.AccountNumber))
                await _emailService.SendEmail(MailType.AccountTransaction, account);
        }
    }
}
