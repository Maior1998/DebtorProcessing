using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DebtorsProcessing.DatabaseModel.Abstractions;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    /// <summary>
    /// Представляет сеанс пользователя.
    /// </summary>
    public record UserSession : BaseEntity
    {
        /// <summary>
        /// Дата-время открытия сеанса пользователя.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата-время окончания сеанса пользователя.
        /// </summary>
        public DateTime? EndDate { get; set; }

        [Required]
        public Guid? UserId { get; set; }
        /// <summary>
        /// Пользователь, вошедший в систему (открывший сеанс).
        /// </summary>
        public User User { get; set; }
        
        public User UserByActiveSession { get; set; }

        /// <summary>
        /// Список ролей, с которыми пользователь вошел в данный сеанс.
        /// </summary>
        public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
        /// <summary>
        /// Коллекция токенов обновления, с которыми пользователь заходил в данную сессию.
        /// </summary>
        public ICollection<SessionRefreshToken> SessionRefreshTokens { get; set; } = new List<SessionRefreshToken>();
    }
}
