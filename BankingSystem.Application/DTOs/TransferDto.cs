﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Application.DTOs
{
    public class TransferDto
    {
        public string SenderAccountNumber { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }

}
