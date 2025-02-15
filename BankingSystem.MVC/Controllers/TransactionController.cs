using BankingSystem.Core.Entities;  // Reference Transaction model
using BankingSystem.Application.Interfaces;  // Interface for Transaction Service
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BankingSystem.Application.DTOs;

namespace BankingSystem.MVC.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        // GET method for deposit form
        public IActionResult Deposit()
        {
            return View();
        }

        // POST method to handle deposit
        [HttpPost]
        public async Task<IActionResult> Deposit(Transaction transaction)
        {
            var transactionDTo = _mapper.Map<TransactionDto>(transaction);
            if (ModelState.IsValid)
            {
                await _transactionService.Deposit(transactionDTo);
                return RedirectToAction("Index", "Home");
            }
            return View(transaction);
        }
    }
}
