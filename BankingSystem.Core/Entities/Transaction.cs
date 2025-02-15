    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BankingSystem.Core.Enums;

namespace BankingSystem.Core.Entities
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public string TransactionID { get; set; }  // Primary Key (PK)

        [Required(ErrorMessage = "Account number is required")]
        public string AccountNumber { get; set; }  // Foreign Key (FK)

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Transaction type is required")]
        public TransactionType TransactionType { get; set; }  // Credit/Debit (Enum)

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        public decimal BalanceAfterTransaction { get; set; }

        public Account Account { get; set; }  // Navigation property
    }
}
