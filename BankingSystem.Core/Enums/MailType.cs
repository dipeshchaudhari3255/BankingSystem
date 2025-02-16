using System.ComponentModel;

namespace BankingSystem.Core.Enums
{
    public enum MailType
    {
        [Description("Account Created")]
        AccountCreation = 1,

        [Description("Account Updated")]
        AccountUpdate,

        [Description("Account Deleted")]
        AccountDeletion,

        [Description("Account Transaction")]
        AccountTransaction,

        [Description("Account Balance")]
        AccountBalance,

        [Description("Account Statement")]
        AccountStatement
    }
}
