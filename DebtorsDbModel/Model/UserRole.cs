using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsDbModel.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; } 
        public ICollection<RoleObjectAccess> RoleObjectAccesses { get; set; } 
    }
}
