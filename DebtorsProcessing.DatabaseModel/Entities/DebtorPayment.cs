using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    /// <summary>
    /// Представляет собой платеж по должнику.
    /// </summary>
    public record DebtorPayment
    {
        /// <summary>
        /// Номер записи в базе данных.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Сумма этого платежа.
        /// </summary>
        public decimal Amount { get; set; } = 0;
        /// <summary>
        /// Дата проведения этого платежа.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;
        /// <summary>
        /// Должник, по которому выполнен данный платеж. Является обязательным в БД.
        /// </summary>
        [Required] public Debtor Debtor { get; set; }
    }
}
