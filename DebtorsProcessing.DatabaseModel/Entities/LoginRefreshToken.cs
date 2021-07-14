using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    public record LoginRefreshToken
    {
        /// <summary>
        /// Номер записи в базе данных.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Строковое представление данного токена обновления (его Guid).
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Id токена, которого призван обновить данный токен обновления.
        /// </summary>
        public string JwtId { get; set; }
        /// <summary>
        /// Определяет, был ли уже использован данный токен обновления.
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// Определяет, был ли отозван данный токен обновления.
        /// </summary>
        public bool IsRevoked { get; set; }
        /// <summary>
        /// Определяет время создания данного токена обновления.
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Определяет срок действия данного токена обновления.
        /// </summary>
        public DateTime ExpiryTime { get; set; }
        /// <summary>
        /// Id пользователя, запросившего токен.
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// Пользователь, запросивший токен.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
