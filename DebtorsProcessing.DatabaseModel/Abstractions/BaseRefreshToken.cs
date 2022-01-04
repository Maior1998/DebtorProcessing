using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsProcessing.DatabaseModel.Abstractions
{
    public abstract record BaseRefreshToken<T> : BaseEntity where T : BaseEntity
    {
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
        /// Определяет срок действия данного токена обновления.
        /// </summary>
        public DateTime ExpiryTime { get; set; }
        /// <summary>
        /// Id пользователя, запросившего токен.
        /// </summary>
        public Guid RecordId { get; set; }
        /// <summary>
        /// Сессия пользователя, для которой был создан данный токен.
        /// </summary>
        [ForeignKey(nameof(RecordId))]
        public T Record { get; set; }
    }
}
