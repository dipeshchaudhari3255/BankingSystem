using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;

namespace BankingSystem.infrastructure.Interfaces
{
    public interface IEmailRepository
    {
        Task SendEmail(MailType subject, Account account);
        Task<bool> IsEmailNotificationEnabled(string accountNumber);
    }
}
