using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;

namespace BankingSystem.Core.Common
{
    public static class Helper
    {
        // Generate unique Account Number
        public static string GenerateAccountNumber()
        {
            var random = new Random();
            var timestamp = DateTime.UtcNow.Ticks.ToString().Substring(5, 10);

            var randomDigits = random.Next(100000, 999999).ToString();

            var accountNumber = timestamp + randomDigits;

            return accountNumber;
        }

        public static string GenerateTransactionReference()
        {
            var random = new Random();
            var timestamp = DateTime.UtcNow.Ticks.ToString().Substring(5, 10);
            var randomDigits = random.Next(100000, 999999).ToString();
            var transactionReference = timestamp + randomDigits;
            return transactionReference;
        }


        public static string GenerateHtml(MailType mailType, Account account)
        {
            string emailBody = string.Empty;

            switch (mailType)
            {
                case MailType.AccountCreation:
                    emailBody = $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Account Created</title>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .highlight {{ background-color: yellow; font-weight: bold; }}
                </style>
            </head>
            <body>
                <h2>Account Successfully Created</h2>
                <p>Dear <span class=""highlight"">{account.Name}</span>,</p>
                <p>Your account has been successfully created on <span class=""highlight"">{DateTime.Today}</span>.</p>
                <p>Account Number: <span class=""highlight"">{account.AccountNumber}</span></p>
                <p>Thank you for banking with us.</p>
            </body>
            </html>";
                    break;

                case MailType.AccountUpdate:
                    emailBody = $@"
            <html>
            <body>
                <h2>Account Updated</h2>
                <p>Dear <span class=""highlight"">{account.Name}</span>,</p>
                <p>Your account details have been successfully updated.</p>
                <p>If you did not request this change, please contact support immediately.</p>
            </body>
            </html>";
                    break;

                case MailType.AccountDeletion:
                    emailBody = $@"
            <html>
            <body>
                <h2>Account Deleted</h2>
                <p>Dear <span class=""highlight"">{account.Name}</span>,</p>
                <p>Your account has been permanently deleted as per your request.</p>
                <p>We hope to serve you again in the future.</p>
            </body>
            </html>";
                    break;

                case MailType.AccountTransaction:
                    emailBody = $@"
            <html>
            <body>
                <h2>Account Transaction Alert</h2>
                <p>Dear <span class=""highlight"">{account.Name}</span>,</p>
                <p>A recent transaction has been made on your account.</p>
                <p>Amount: <span class=""highlight"">{account.Balance:C}</span></p>
                <p>Remaining Balance: <span class=""highlight"">{account.Balance:C}</span></p>
            </body>
            </html>";
                    break;

                case MailType.AccountBalance:
                    emailBody = $@"
            <html>
            <body>
                <h2>Account Balance Update</h2>
                <p>Dear <span class=""highlight"">{account.Name}</span>,</p>
                <p>Your current account balance is: <span class=""highlight"">{account.Balance:C}</span></p>
            </body>
            </html>";
                    break;

                case MailType.AccountStatement:
                    emailBody = $@"
            <html>
            <body>
                <h2>Monthly Account Statement</h2>
                <p>Dear <span class=""highlight"">{account.Name}</span>,</p>
                <p>Your monthly statement is ready. Please find the details below.</p>
                <p>Account Number: <span class=""highlight"">{account.AccountNumber}</span></p>
                <p>Closing Balance: <span class=""highlight"">{account.Balance:C}</span></p>
                <p>Check your full statement in the banking portal.</p>
            </body>
            </html>";
                    break;

                default:
                    emailBody = "<p>Invalid mail type</p>";
                    break;
            }

            return emailBody;
        }


    }
}
