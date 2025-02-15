using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.infrastructure.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
