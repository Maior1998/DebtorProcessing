using System;


namespace DebtorsDbModel.Model
{
    /// <summary>
    /// Представляет собой запись о том, что указанная роль имеет указанное право на указанный объект.
    /// </summary>
    public class RoleObjectAccess
    {
        /// <summary>
        /// Номер записи в базе данных.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор роли. Служит внешним ключом для работы <see cref="UserRole"/>.
        /// </summary>
        public Guid UserRoleId { get; set; }

        /// <summary>
        /// Роль, имеющая доступ к объекту.
        /// </summary>
        public UserRole UserRole { get; set; }

        /// <summary>
        /// Уникальный идентификатор объекта доступа. Служит внешним ключом для работы <see cref="Object"/>.
        /// </summary>
        public Guid ObjectId { get; set; }

        /// <summary>
        /// Объект, к которому установлен доступ роли.
        /// </summary>
        public SecurityObject Object { get; set; }

    }



}
