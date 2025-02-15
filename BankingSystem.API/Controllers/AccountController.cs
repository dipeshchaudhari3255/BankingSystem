using BankingSystem.Application.DTOs;
using System.Security.Principal;
using BankingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Create Account
        [HttpPost]
        public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] AccountDto accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdAccount = await _accountService.CreateAccount(accountDto);
                return CreatedAtAction(nameof(GetAccount), new { accountNumber = createdAccount.AccountNumber }, createdAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, details = ex.StackTrace });
            }
        }

        // Get Account by Account Number
        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<AccountDto>> GetAccount(string accountNumber)
        {
            var account = await _accountService.GetAccount(accountNumber);
            if (account == null)
                return NotFound();
            return Ok(account);
        }

        // Get All Accounts
        [HttpGet("GetAccounts")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
        {
            var accounts = await _accountService.GetAccounts();
            if (accounts == null || !accounts.Any())
                return NotFound("No accounts found.");
            return Ok(accounts);
        }
    }

}
