using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;
using BankingSystem.infrastructure.Interfaces;

namespace BankingSystem.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public TransactionService(IAccountRepository accountRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        // Deposit Funds
        public async Task<TransactionDto> Deposit(TransactionDto transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            var account = await _accountRepository.GetAccount(transaction.AccountNumber);

            // Check if account exists
            if (account == null)
                throw new Exception("Account not found.");

            account.Balance += transactionDto.Amount;

            // Save transaction and update account balance
            await _transactionRepository.CreateTransaction(transaction);
            await _accountRepository.UpdateAccount(account);

            // Return the updated transaction as DTO
            return _mapper.Map<TransactionDto>(transaction);
        }

        // Withdraw Funds
        public async Task<TransactionDto> Withdraw(TransactionDto transactionDto)
        {
            var account = await _accountRepository.GetAccount(transactionDto.AccountNumber);

            // Check if account exists
            if (account == null)
                throw new Exception("Account not found.");

            // Check if there are sufficient funds
            if (account.Balance < transactionDto.Amount)
                throw new Exception("Insufficient balance.");

            account.Balance -= transactionDto.Amount;

            var transaction = new Transaction
            {
                TransactionID = Guid.NewGuid().ToString(),
                AccountNumber = transactionDto.AccountNumber,
                Amount = transactionDto.Amount,
                TransactionType = TransactionType.Debit, // Assuming Debit for withdrawals
                BalanceAfterTransaction = account.Balance,
                TransactionDate = DateTime.UtcNow
            };

            // Save transaction and update account balance
            await _transactionRepository.CreateTransaction(transaction);
            await _accountRepository.UpdateAccount(account);

            // Return the updated transaction as DTO
            return new TransactionDto
            {
                TransactionID = Guid.NewGuid().ToString(),
                AccountNumber = transaction.AccountNumber,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                BalanceAfterTransaction = transaction.BalanceAfterTransaction,
                TransactionDate = transaction.TransactionDate
            };
        }


        // Transfer Money Logic
        public async Task<TransactionDto> Transfer(TransferDto transferDto)
        {
            var senderAccount = await _accountRepository.GetAccount(transferDto.SenderAccountNumber);
            var receiverAccount = await _accountRepository.GetAccount(transferDto.ReceiverAccountNumber);

            // Ensure both sender and receiver exist
            if (senderAccount == null)
                throw new Exception("Sender account not found.");
            if (receiverAccount == null)
                throw new Exception("Receiver account not found.");

            // Check if sender has enough balance
            if (senderAccount.Balance < transferDto.Amount)
                throw new Exception("Insufficient balance for the transfer.");

            // Debit the sender's account
            senderAccount.Balance -= transferDto.Amount;
            var senderTransaction = new Transaction
            {
                TransactionID = Guid.NewGuid().ToString(),
                AccountNumber = senderAccount.AccountNumber,
                Amount = transferDto.Amount,
                TransactionType = TransactionType.Debit,
                BalanceAfterTransaction = senderAccount.Balance,
                TransactionDate = DateTime.UtcNow
            };

            // Credit the receiver's account
            receiverAccount.Balance += transferDto.Amount;
            var receiverTransaction = new Transaction
            {
                TransactionID = Guid.NewGuid().ToString(),
                AccountNumber = receiverAccount.AccountNumber,
                Amount = transferDto.Amount,
                TransactionType = TransactionType.Credit,
                BalanceAfterTransaction = receiverAccount.Balance,
                TransactionDate = DateTime.UtcNow
            };

            // Save both transactions and update accounts
            await _transactionRepository.CreateTransaction(senderTransaction);
            await _transactionRepository.CreateTransaction(receiverTransaction);
            await _accountRepository.UpdateAccount(senderAccount);
            await _accountRepository.UpdateAccount(receiverAccount);

            // Return the sender's transaction as DTO
            return new TransactionDto
            {
                TransactionID = Guid.NewGuid().ToString(),
                AccountNumber = senderTransaction.AccountNumber,
                Amount = senderTransaction.Amount,
                TransactionType = senderTransaction.TransactionType,
                BalanceAfterTransaction = senderTransaction.BalanceAfterTransaction,
                TransactionDate = senderTransaction.TransactionDate
            };
        }

        public async Task<List<Transaction>> GetTransactionsByAccountNumber(string accountNumber)
        {
            var transactions = await _transactionRepository.GetTransactionsByAccountNumber(accountNumber);
            return transactions;
        }

        public async Task<List<DailyTransactionSummaryDto>> GetDailyTransactionSummary()
        {
            var transactions = await _transactionRepository.GetAllTransactions();

            var dailySummary = transactions
                .GroupBy(t => t.TransactionDate.Date)
                .Select(g => new DailyTransactionSummaryDto
                {
                    Date = g.Key,
                    TotalDeposits = g.Where(t => t.TransactionType == TransactionType.Credit).Sum(t => t.Amount),
                    TotalWithdrawals = g.Where(t => t.TransactionType == TransactionType.Debit).Sum(t => t.Amount),
                    TotalBalance = g.LastOrDefault()?.BalanceAfterTransaction ?? 0
                })
                .Where(x => x.Date == DateTime.Today)
                .OrderBy(d => d.Date)
                .ToList();

            return dailySummary;
        }

    }
}
