using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    public record SessionRefreshToken
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
        public Guid SessionId { get; set; }
        /// <summary>
        /// Сессия пользователя, для которой был создан данный токен.
        /// </summary>
        [ForeignKey(nameof(SessionId))]
        public UserSession Session { get; set; }
    }
}
