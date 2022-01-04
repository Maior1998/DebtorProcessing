using System;
using System.Collections.Generic;

using DebtorsProcessing.DatabaseModel.Abstractions;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    /// <summary>
    /// Представляет собой роль пользователя системы.
    /// </summary>
    public record UserRole : BaseLookup
    {

        /// <summary>
        /// Список пользователей, имеющих данную роль.
        /// </summary>
        public ICollection<User> Users { get; set; } = new List<User>();

        /// <summary>
        /// Объекты, к которым имеет доступ данная роль.
        /// </summary>
        public ICollection<SecurityObject> Objects { get; set; } = new List<SecurityObject>();

        public ICollection<UserSession> UsedInSessions { get; set; } = new List<UserSession>();


        
    }
}
