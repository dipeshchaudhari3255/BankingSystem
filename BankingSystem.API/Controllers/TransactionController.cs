using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController(ITransactionService _transactionService, ILogger<TransactionController> _logger, IMapper _mapper) : ControllerBase
    {
        [HttpPost("deposit")]
        [ProducesResponseType(typeof(TransactionDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<TransactionDto>> Deposit([FromBody] TransactionDto transactionDto)
        {
            if (transactionDto.Amount <= 0)
            {
                return BadRequest("Deposit amount must be greater than zero.");
            }
            try
            {
                var result = await _transactionService.Deposit(transactionDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing deposit transaction.");
                return StatusCode(500, new { message = "An error occurred while processing the transaction.", details = ex.Message });
            }
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<TransactionDto>> Withdraw([FromBody] TransactionDto transactionDto)
        {
            if (transactionDto.Amount <= 0)
            {
                return BadRequest("Withdraw amount must be greater than zero.");
            }
            try
            {
                var result = await _transactionService.Withdraw(transactionDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing withdraw transaction.");
                return StatusCode(500, new { message = "An error occurred while processing the transaction.", details = ex.Message });
            }
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<TransactionDto>> Transfer([FromBody] TransferDto transferDto)
        {
            if (transferDto.Amount <= 0)
            {
                return BadRequest("Transfer amount must be greater than zero.");
            }
            try
            {
                var result = await _transactionService.Transfer(transferDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing transfer transaction.");
                return StatusCode(500, new { message = "An error occurred while processing the transaction.", details = ex.Message });
            }
        }

        [HttpGet("account-statement/{accountNumber}")]
        public async Task<ActionResult<List<TransactionDto>>> GetAccountStatement(string accountNumber)
        {
            var transactions = await _transactionService.GetTransactionsByAccountNumber(accountNumber);
            if (transactions == null || !transactions.Any())
                return NotFound("No transactions found for this account.");

            var transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);
            return Ok(transactionDtos);
        }

        [HttpGet("daily-transaction-summary")]
        public async Task<ActionResult<List<DailyTransactionSummaryDto>>> GetDailyTransactionSummary()
        {
            var dailySummary = await _transactionService.GetDailyTransactionSummary();
            return Ok(dailySummary);
        }
    }
}
