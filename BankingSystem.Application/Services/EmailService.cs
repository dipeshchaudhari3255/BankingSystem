using BankingSystem.Application.Interfaces;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;
using BankingSystem.infrastructure.Interfaces;

namespace BankingSystem.Application.Services
{
    public class EmailService(IEmailRepository _emailRepository) : IEmailService
    {

        public Task<bool> IsEmailNotificationEnabled(string accountNumber)
        {
            return _emailRepository.IsEmailNotificationEnabled(accountNumber);
        }

        public Task SendEmail(MailType subject, Account account)
        {
            return _emailRepository.SendEmail(subject, account);
        }
    }
}
