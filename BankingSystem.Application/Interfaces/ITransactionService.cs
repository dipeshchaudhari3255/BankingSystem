using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Application.DTOs;
using BankingSystem.Core.Entities;

namespace BankingSystem.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> Deposit(TransactionDto transactionDto);
        Task<TransactionDto> Withdraw(TransactionDto transactionDto);
        Task<TransactionDto> Transfer(TransferDto transferDto);
        Task<List<Transaction>> GetTransactionsByAccountNumber(string accountNumber);
        Task<List<DailyTransactionSummaryDto>> GetDailyTransactionSummary();
    }
}
