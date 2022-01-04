using System;
using System.ComponentModel.DataAnnotations;

namespace DebtorsProcessing.DatabaseModel.Abstractions
{
    public abstract record BaseEntity
    {
        [Key] public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
