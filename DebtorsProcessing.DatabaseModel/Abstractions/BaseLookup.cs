using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsProcessing.DatabaseModel.Abstractions
{
    public abstract record BaseLookup : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
