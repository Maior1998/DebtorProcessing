using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsDbModel.Model
{
    /// <summary>
    /// Представляет сеанс пользователя.
    /// </summary>
    public record UserSession
    {
        /// <summary>
        /// Номер записи в базе данных.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата-время открытия сеанса пользователя.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата-время окончания сеанса пользователя.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Пользователь, вошедший в систему (открывший сеанс).
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Список ролей, с которыми пользователь вошел в данный сеанс.
        /// </summary>
        public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
