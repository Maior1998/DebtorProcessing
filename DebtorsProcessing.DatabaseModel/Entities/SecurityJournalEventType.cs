using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    [Table("SecurityJournalEventTypes")]
    public class SecurityJournalEventType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<SecurityJournalEvent> Events { get; set; } = new List<SecurityJournalEvent>();
    }
}
