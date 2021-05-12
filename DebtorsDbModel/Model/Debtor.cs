using System;
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
        public ushort PassportSeries { get; set; }
        public ushort PassportNumber { get; set; }
        public string FullName { get; set; }
        public string RegistrationAddress { get; set; }
        public decimal StartDebt { get; set; }
        public List<DebtorPayment> Payments { get; set; } = new();
        public decimal CurrentDebt => StartDebt - Payments.Sum(x => x.Amount);
    }
}
