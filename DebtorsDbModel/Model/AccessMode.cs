using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsDbModel.Model
{
    public class AccessMode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoleObjectAccess> RoleObjectAccesses { get; set; } = new List<RoleObjectAccess>();
    }
}
