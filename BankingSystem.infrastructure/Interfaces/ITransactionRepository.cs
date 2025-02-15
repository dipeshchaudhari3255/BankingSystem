using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Entities;

namespace BankingSystem.infrastructure.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateTransaction(Transaction transaction);
        Task<List<Transaction>> GetAllTransactions();
        Task<List<Transaction>> GetTransactionsByAccountNumber(string accountNumber);
    }
}
