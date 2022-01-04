using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;
using DebtorsProcessing.Desktop.Services;
using DebtorsProcessing.Desktop.View.Windows;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class AdminPanelUsersManagementViewModel : ReactiveObject
    {
        private DelegateCommand addRoleToUser;


        private DelegateCommand addUser;

        private Guid[] availiableRolesToUse = new Guid[0];

        private DelegateCommand deleteUser;

        private DelegateCommand revokeRoleFromUser;
        private readonly SessionService session;

        public AdminPanelUsersManagementViewModel(SessionService session)
        {
            this.session = session;
            UpdateAvailiableRoles();
            LoadUsers();
        }

        [Reactive] public ICollection<User> Users { get; set; }
        [Reactive] public User SelectedUser { get; set; }
        [Reactive] public UserRole SelectedUserRole { get; set; }

        public DelegateCommand DeleteUser => deleteUser ??= new(() =>
        {
            if (MessageBox.Show($"Вы действительно хотите удалить сотрудника {SelectedUser.FullName}?",
                    "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;
            DebtorsContext db = new();
            User deletingUser = db.Users.Single(x => x.Id == SelectedUser.Id);
            db.Users.Remove(deletingUser);
            db.SaveChanges();
            Users.Remove(SelectedUser);
            OnUsersChanged?.Invoke();
        }, () => SelectedUser != null && SelectedUser.Id != session.UserId);

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
                //PasswordHash = User.GetHashedString(editUserWindow.Password)
            };
            DebtorsContext db = new();
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


        private DelegateCommand changeUserPassword;
        public DelegateCommand ChangeUserPassword => changeUserPassword ??= new(() =>
        {
            ChangePasswordWindow changePasswordWindow = new();
            if (!changePasswordWindow.ShowDialog().Value) return;
            string hash = null;
            DebtorsContext db = new();
            User user = db.Users.Single(x => x.Id == SelectedUser.Id);
            user.PasswordHash = hash;
            db.SaveChanges();
            MessageBox.Show(
                "Пароль успешно установлен",
                "Изменение пароля пользователя",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

        }, () => SelectedUser != null);

        public DelegateCommand AddRoleToUser => addRoleToUser ??= new(() =>
        {
            ChooseUserRoleWindow chooseUserRoleWindow = new()
            {
                ExcludingRoles = SelectedUser.UserRoles.Select(x => x.Id).ToArray()
            };
            if (!chooseUserRoleWindow.ShowDialog().Value) return;
            DebtorsContext db = new();
            User editingUser = db.Users.Single(x => x.Id == SelectedUser.Id);
            UserRole addingRole = db.UserRoles.Single(x => x.Id == chooseUserRoleWindow.SelectedUserRole.Id);
            editingUser.UserRoles.Add(addingRole);
            db.SaveChanges();
            SelectedUser.UserRoles.Add(chooseUserRoleWindow.SelectedUserRole);
            OnUserRolesChanged?.Invoke();
        }, () =>
            SelectedUser != null
            && SelectedUser.Id != session.UserId
            && availiableRolesToUse.Except(SelectedUser.UserRoles.Select(x => x.Id)).Any());

        public DelegateCommand RevokeRoleFromUser => revokeRoleFromUser ??= new(() =>
        {
            if (MessageBox.Show(
                    $"Вы действительно хотите отозвать роль {SelectedUserRole.Name} у сотрудника {SelectedUser.FullName}?",
                    "Отзыв роли", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;
            DebtorsContext db = new();
            User user = db.Users.Include(x => x.UserRoles).Single(x => x.Id == SelectedUser.Id);
            UserRole userRole = db.UserRoles.Single(x => x.Id == SelectedUserRole.Id);
            user.UserRoles.Remove(userRole);
            db.SaveChanges();
            SelectedUser.UserRoles.Remove(SelectedUserRole);
            OnUserRolesChanged?.Invoke();
        }, () => SelectedUserRole != null);

        private void UpdateAvailiableRoles()
        {
            DebtorsContext db = new();
            availiableRolesToUse = db.UserRoles.Select(x => x.Id).ToArray();
        }

        private void LoadUsers()
        {
            DebtorsContext context = new();

            Users = context.Users.Include(x => x.UserRoles).Where(x => x.Id != session.UserId)
                .ToList();
        }

        public event Action OnUserRolesChanged;
        public event Action OnUsersChanged;
    }
}