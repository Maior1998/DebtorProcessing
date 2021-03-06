using System;
using System.ComponentModel.DataAnnotations;

using DebtorsProcessing.DatabaseModel.Abstractions;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    /// <summary>
    /// Представляет собой платеж по должнику.
    /// </summary>
    public record DebtorPayment : BaseEntity
    {
        /// <summary>
        /// Сумма этого платежа.
        /// </summary>
        public decimal Amount { get; set; } = 0;
        /// <summary>
        /// Дата проведения этого платежа.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;


        [Required]
        public Guid? DebtorId { get; set; }
        /// <summary>
        /// Должник, по которому выполнен данный платеж. Является обязательным в БД.
        /// </summary>
        public Debtor Debtor { get; set; }
    }
}
