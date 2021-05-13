using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DebtorsDbModel.Model
{
    /// <summary>
    /// Представляет собой запись о том, что указанная роль имеет указанное правл на указанный объект.
    /// </summary>
    public class RoleObjectAccess
    {
        public Guid Id { get; set; }
        public Guid UserRoleId { get; set; }
        public UserRole UserRole { get; set; }

        public Guid ObjectId { get; set; }
        public SecurityObject Object { get; set; }

        public Guid GrantedAccessModeId { get; set; }

        public AccessMode GrantedAccessMode { get; set; }

    }


    public class AccessMode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoleObjectAccess> RoleObjectAccesses { get; set; } = new List<RoleObjectAccess>();
    }
}
