using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DebtorsProcessing.DatabaseModel.Abstractions;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    public record DebtorComment : BaseEntity
    {
        public Guid AuthorId { get; set; }
        public User Author { get; set; }

        public Guid DebtorId { get; set; }
        public Debtor Debtor { get; set; }

        public string Comment { get; set; }
    }
}
