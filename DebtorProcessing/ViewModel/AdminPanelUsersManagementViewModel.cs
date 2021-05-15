using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DebtorProcessing.Services;
using DebtorProcessing.View;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class AdminPanelUsersManagementViewModel : ReactiveObject
    {
        private SessionService session;
        public AdminPanelUsersManagementViewModel(SessionService session)
        {
            this.session = session;
            UpdateAvailiableRoles();
            LoadUsers();
        }

        private void UpdateAvailiableRoles()
        {
            Context db = new();
            availiableRolesToUse = db.UserRoles.Select(x => x.Id).ToArray();
        }

        private Guid[] availiableRolesToUse = new Guid[0];

        private void LoadUsers()
        {
            Context context = new();

            Users = context.Users.Include(x => x.UserRoles).Where(x => x.Id != session.CurrentLoggedInUser.Id)
                .ToList();
        }

        [Reactive] public ICollection<User> Users { get; set; }
        [Reactive] public User SelectedUser { get; set; }
        [Reactive] public UserRole SelectedUserRole { get; set; }

        private DelegateCommand deleteUser;

        public DelegateCommand DeleteUser => deleteUser ??= new(() =>
        {
            if (MessageBox.Show($"Вы действительно хотите удалить сотрудника {SelectedUser.FullName}?", "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;
            Context db = new();
            User deletingUser = db.Users.Single(x => x.Id == SelectedUser.Id);
            db.Users.Remove(deletingUser);
            db.SaveChanges();
            Users.Remove(SelectedUser);
            OnUsersChanged?.Invoke();

        }, () => SelectedUser != null && SelectedUser.Id != session.CurrentLoggedInUser.Id);


        private DelegateCommand addUser;

        public DelegateCommand AddUser => addUser ??= new(() =>
        {
            EditUserWindow editUserWindow = new()
            {
                IsPassordEditModeEnabled = true
            };

            if (!editUserWindow.ShowDialog().Value) return;
            User addingUser = new()
            {
                FullName = editUserWindow.FullName,
                Login = editUserWindow.Login,
                PasswordHash = User.GetHashedString(editUserWindow.Password)
            };
            Context db = new();
            if (db.Users.Any(x => x.Login.ToLower() == addingUser.Login.ToLower()))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            db.Users.Add(addingUser);
            db.SaveChanges();
            Users.Add(addingUser);
            OnUsersChanged?.Invoke();
        });

        private DelegateCommand addRoleToUser;

        public DelegateCommand AddRoleToUser => addRoleToUser ??= new(() =>
        {
            ChooseUserRoleWindow chooseUserRoleWindow = new()
            {
                ExcludingRoles = SelectedUser.UserRoles.Select(x => x.Id).ToArray()
            };
            if (!chooseUserRoleWindow.ShowDialog().Value) return;
            Context db = new();
            User editingUser = db.Users.Single(x => x.Id == SelectedUser.Id);
            UserRole addingRole = db.UserRoles.Single(x => x.Id == chooseUserRoleWindow.SelectedUserRole.Id);
            editingUser.UserRoles.Add(addingRole);
            db.SaveChanges();
            SelectedUser.UserRoles.Add(chooseUserRoleWindow.SelectedUserRole);
            OnUserRolesChanged?.Invoke();
        }, () =>
            SelectedUser != null
            && SelectedUser.Id != session.CurrentLoggedInUser.Id
            && availiableRolesToUse.Except(SelectedUser.UserRoles.Select(x => x.Id)).Any());

        private DelegateCommand revokeRoleFromUser;

        public DelegateCommand RevokeRoleFromUser => revokeRoleFromUser ??= new(() =>
        {
            if (MessageBox.Show($"Вы действительно хотите отозвать роль {SelectedUserRole.Name} у сотрудника {SelectedUser.FullName}?", "Отзыв роли", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;
            Context db = new();
            User user = db.Users.Include(x => x.UserRoles).Single(x => x.Id == SelectedUser.Id);
            UserRole userRole = db.UserRoles.Single(x => x.Id == SelectedUserRole.Id);
            user.UserRoles.Remove(userRole);
            db.SaveChanges();
            SelectedUser.UserRoles.Remove(SelectedUserRole);
            OnUserRolesChanged?.Invoke();
        }, () => SelectedUserRole != null);

        public event Action OnUserRolesChanged;
        public event Action OnUsersChanged;

    }
}
