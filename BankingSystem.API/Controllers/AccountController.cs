using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService _accountService, ILogger<AccountController> _logger) : ControllerBase
    {
        // Create Account
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountDto accountDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdAccount = await _accountService.CreateAccount(accountDto);
                return CreatedAtAction(nameof(GetAccount), new { accountNumber = createdAccount.AccountNumber }, createdAccount);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business logic exception occurred while creating an account.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating an account.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        // Get Account by Account Number
        [HttpGet("{accountNumber}")]
        public async Task<IActionResult> GetAccount(string accountNumber)
        {
            try
            {
                var account = await _accountService.GetAccount(accountNumber);
                if (account == null)
                    return NotFound(new { message = "Account not found." });

                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account details.");
                return StatusCode(500, new { message = "An error occurred while fetching account details." });
            }
        }

        // Get All Accounts
        [HttpGet("all")]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                var accounts = await _accountService.GetAccounts();
                if (accounts == null || !accounts.Any())
                    return NotFound(new { message = "No accounts found." });

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all accounts.");
                return StatusCode(500, new { message = "An error occurred while retrieving account records." });
            }
        }
    }
}
