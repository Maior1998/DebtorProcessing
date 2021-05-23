using System;
using System.Linq;
using DebtorsDbModel.Model;

namespace DebtorProcessing.Services
{
    public class SessionService
    {
        private Guid[] accessesObjects;

        private User currentLoggedInUser;

        /// <summary>
        ///     Текущий залогиненный в систему пользователь.
        /// </summary>
        public User CurrentLoggedInUser
        {
            get => currentLoggedInUser;
            set
            {
                currentLoggedInUser = value;
                accessesObjects = CurrentLoggedInUser.UserRoles.Aggregate(Enumerable.Empty<Guid>(),
                    (current, nextRole) => current.Union(nextRole.Objects.Select(x => x.Id))).ToArray();
                HasAccessToAdminPanel = CheckRight("Доступ к панели администрирования");
                CanEditNotOwnedDebtorsData =
                    CheckRight("Доступ на изменения данных должников, по которым сотрудник не является ответственным");
                CanEditDebtorsTable = CheckRight("Добавление или удаление должников");
                CanViewNotOwnedDebtors = CheckRight("Просмотр не своих должников");
                CanTransferDebtors = CheckRight("Перезакрепление должников на между сотрудниками");
                CanUserChangeTheirPassword = CheckRight("Может изменять свой пароль");
            }
        }

        public bool CanEditDebtorsTable { get; private set; }
        public bool HasAccessToAdminPanel { get; private set; }
        public bool CanEditNotOwnedDebtorsData { get; private set; }
        public bool CanViewNotOwnedDebtors { get; private set; }
        public bool CanTransferDebtors { get; private set; }
        public bool CanUserChangeTheirPassword { get; private set; }

        private bool CheckRight(string rightName)
        {
            return accessesObjects.Any(x => x == SecurityObject.ObjectNameToIdTranslator[rightName]);
        }
    }
}