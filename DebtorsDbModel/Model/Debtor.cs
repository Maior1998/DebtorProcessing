﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsDbModel.Model
{
    public class Debtor
    {
        public Guid Id { get; set; }
        public User Responsible { get; set; }
        public string ContractNumber { get; set; }
        public ushort PassportSeries { get; set; }
        public uint PassportNumber { get; set; }
        public string FullName { get; set; }
        public string RegistrationAddress { get; set; }
        public decimal StartDebt { get; set; }
        public ICollection<DebtorPayment> Payments { get; set; } = new List<DebtorPayment>();
        public decimal CurrentDebt => StartDebt - Payments.Sum(x => x.Amount);
    }
}
