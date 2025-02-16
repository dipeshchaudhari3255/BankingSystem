using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;

namespace BankingSystem.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(MailType subject, Account account);
        Task<bool> IsEmailNotificationEnabled(string accountNumber);
    }
}
