using BankingSystem.Core.Entities; 
using BankingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BankingSystem.Application.DTOs;

namespace BankingSystem.MVC.Controllers
{
    public class AccountController : Controller
    {
        // GET method to show account creation form
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
