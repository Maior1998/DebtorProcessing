using System;
using System.Collections.Generic;
using System.Linq;

using DebtorsProcessing.DatabaseModel.Abstractions;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    /// <summary>
    /// Представляет собой должника.
    /// </summary>
    public record Debtor : BaseEntity
    {
        public Guid? ResponsibleId { get; set; }

        /// <summary>
        /// Ответственный за данного должника сотрудник.
        /// </summary>
        public User Responsible { get; set; }

        /// <summary>
        /// Номер договора кредита данного должника.
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// ФИО данного должника.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Id региона регистрации должника.
        /// </summary>
        public Guid? RegistrationRegionId { get; set; }
        /// <summary>
        /// Регион регистрации должника.
        /// </summary>
        public Region RegistrationRegion { get; set; }

        /// <summary>
        /// Стартовый долг должника.
        /// </summary>
        public decimal StartDebt { get; set; }

        /// <summary>
        /// Список платежей данного должника.
        /// </summary>
        public ICollection<DebtorPayment> Payments { get; set; } = new List<DebtorPayment>();

        /// <summary>
        /// Текущий долг данного должника. Расчитывается как разница между стартовым долгом и суммой всех платежей данного должника.
        /// </summary>
        public decimal CurrentDebt => StartDebt - Payments.Sum(x => x.Amount);
    }
}
