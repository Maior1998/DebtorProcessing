using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    [Table("SecurityJournalEvents")]
    public class SecurityJournalEvent
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime? CreatedOn { get; set; }
        public Guid? RelatedUserId { get; set; }
        public User RelatedUser { get; set; }
        [Required]
        public Guid? TypeId { get; set; }
        public SecurityJournalEventType Type { get; set; }
        public string Description { get; set; }
    }
}
