using System.Net.Mail;
using System.Net;
using BankingSystem.infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using BankingSystem.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Common;

namespace BankingSystem.infrastructure.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public EmailRepository(IConfiguration config, ApplicationDBContext context)
        {
            _context = context;

            _smtpServer = config["EmailSettings:SmtpServer"] ?? throw new ArgumentNullException(nameof(config), "SmtpServer is missing in configuration.");
            _port = int.TryParse(config["EmailSettings:Port"], out int parsedPort) ? parsedPort : throw new ArgumentException("Invalid SMTP port.", nameof(config));
            _senderEmail = config["EmailSettings:SenderEmail"] ?? throw new ArgumentNullException(nameof(config), "SenderEmail is missing in configuration.");
            _senderPassword = config["EmailSettings:SenderPassword"] ?? throw new ArgumentNullException(nameof(config), "SenderPassword is missing in configuration.");
        }

        public async Task<bool> IsEmailNotificationEnabled(string accountNumber)
        {
            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            return account?.IsEmailNotificationEnabled ?? false;
        }

        public async Task SendEmail(MailType subject, Account account)
        {
            if (string.IsNullOrEmpty(account.Email))
                throw new ArgumentException("Account email is required.");

            try
            {
                using var client = new SmtpClient(_smtpServer, _port)
                {
                    Credentials = new NetworkCredential(_senderEmail, _senderPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = subject.ToString(),
                    Body = Helper.GenerateHtml(subject, account),
                    IsBodyHtml = true
                };

                mailMessage.To.Add(account.Email);

                await client.SendMailAsync(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                // Log exception (if logging is available)
                throw new InvalidOperationException($"Failed to send email: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                // Log exception (if logging is available)
                throw new InvalidOperationException($"Unexpected error sending email: {ex.Message}");
            }
        }
    }
}
