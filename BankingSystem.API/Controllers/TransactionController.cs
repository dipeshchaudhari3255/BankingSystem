using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using BankingSystem.Application.Services;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;
using BankingSystem.infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;

        public TransactionController(IAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("Deposit")]
        public async Task<ActionResult<TransactionDto>> Deposit([FromForm] TransactionDto transactionDto)
        {
            if (transactionDto.Amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.");
            }
            try
            {
                var result = await _transactionService.Deposit(transactionDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Withdraw")]
        public async Task<ActionResult<TransactionDto>> Withdraw([FromForm] TransactionDto transactionDto)
        {
            if (transactionDto.Amount <= 0)
            {
                throw new ArgumentException("Withdraw amount must be greater than zero.");
            }
            try
            {
                var result = await _transactionService.Withdraw(transactionDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Transfer Money
        [HttpPost]
        [Route("Transfer")]
        public async Task<ActionResult<TransactionDto>> Transfer([FromBody] TransferDto transferDto)
        {
            if (transferDto.Amount <= 0)
            {
                throw new ArgumentException("Transfer amount must be greater than zero.");
            }
            try
            {
                var result = await _transactionService.Transfer(transferDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("AccountStatement/{accountNumber}")]
        public async Task<ActionResult<List<TransactionDto>>> GetAccountStatement(string accountNumber)
        {
            var transactions = await _transactionService.GetTransactionsByAccountNumber(accountNumber);
            if (transactions == null || !transactions.Any())
                return NotFound("No transactions found for this account.");

            var transactionDtos = transactions.Select(t => new TransactionDto
            {
                TransactionID = t.TransactionID,
                AccountNumber = t.AccountNumber,
                TransactionType = t.TransactionType,
                Amount = t.Amount,
                BalanceAfterTransaction = t.BalanceAfterTransaction,
                TransactionDate = t.TransactionDate
            }).ToList();

            return Ok(transactionDtos);
        }

        [HttpGet]
        [Route("DailyTransactionSummary")]
        public async Task<ActionResult<List<DailyTransactionSummaryDto>>> GetDailyTransactionSummary()
        {
            var dailySummary = await _transactionService.GetDailyTransactionSummary();
            return Ok(dailySummary);
        }
    }
}
