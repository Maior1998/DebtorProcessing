using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsDbModel.Model
{
    public class DebtorPayment
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        [Required] public Debtor Debtor { get; set; }
    }
}
