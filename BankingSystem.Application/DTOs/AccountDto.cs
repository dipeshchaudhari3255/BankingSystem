
namespace BankingSystem.Application.DTOs
{
    public class AccountDto
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public decimal Balance { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
