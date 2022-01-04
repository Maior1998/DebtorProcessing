using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DebtorsProcessing.DatabaseModel.Abstractions;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    [Table("SecurityJournalEventTypes")]
    public record SecurityJournalEventType : BaseLookup
    {

        public ICollection<SecurityJournalEvent> Events { get; set; } = new List<SecurityJournalEvent>();
    }
}
