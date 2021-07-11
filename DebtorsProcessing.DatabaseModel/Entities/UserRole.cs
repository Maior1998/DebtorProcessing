﻿using System;
using System.Collections.Generic;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    /// <summary>
    /// Представляет собой роль пользователя системы.
    /// </summary>
    public record UserRole
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
        public ICollection<User> Users { get; set; } = new List<User>();

        /// <summary>
        /// Объекты, к которым имеет доступ данная роль.
        /// </summary>
        public ICollection<SecurityObject> Objects { get; set; } = new List<SecurityObject>();

        public ICollection<UserSession> UsedInSessions { get; set; } = new List<UserSession>();


        public override string ToString()
        {
            return Name;
        }
    }
}
