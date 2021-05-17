using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsDbModel.Model
{
    /// <summary>
    /// Представляет собой роль пользователя системы.
    /// </summary>
    public class UserRole
    {

        /// <summary>
        /// Номер записи в базе данных.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название этой роли.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список пользователей, имеющих данную роль.
        /// </summary>
        public ICollection<User> Users { get; set; }

        /// <summary>
        /// Список объектов, к которым имеет доступ данная роль.
        /// </summary>
        public ICollection<RoleObjectAccess> RoleObjectAccesses { get; set; }
    }
}
