using System;
using System.Collections.Generic;
using System.Linq;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    /// <summary>
    /// Представляет собой должника.
    /// </summary>
    public record Debtor
    {
        /// <summary>
        /// Уникальный номер в базе данных.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ответственный за данного должника сотрудник.
        /// </summary>
        public User Responsible { get; set; }

        /// <summary>
        /// Номер договора кредита данного должника.
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// Серия паспорта.
        /// </summary>
        public ushort PassportSeries { get; set; }

        /// <summary>
        /// Номер паспорта.
        /// </summary>
        public uint PassportNumber { get; set; }

        /// <summary>
        /// ФИО данного должника.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Адрес регистрации данного должника.
        /// </summary>
        public string RegistrationAddress { get; set; }

        /// <summary>
        /// Стартовые долг данного должника.
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
