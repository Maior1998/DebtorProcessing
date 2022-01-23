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
        /// Индивидуальный счет должника в банке, с которого производится списание долга.
        /// </summary>
        public string BankAccount { get; set; }

        public DateTime InWorkFromDate { get; set; }

        /// <summary>
        /// Стартовый долг должника.
        /// </summary>
        public decimal StartDebt { get; set; }

        /// <summary>
        /// Список платежей данного должника.
        /// </summary>
        public ICollection<DebtorPayment> Payments { get; set; } = new List<DebtorPayment>();

        public ICollection<DebtorComment> Comments { get; set; } = new List<DebtorComment>();

    }
}
