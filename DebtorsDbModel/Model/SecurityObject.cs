using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsDbModel.Model
{
    public class SecurityObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoleObjectAccess> RoleObjectAccesses = new List<RoleObjectAccess>();
    }
}
