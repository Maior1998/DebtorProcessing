using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DebtorProcessing.Services;

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
            Context context = new();

            Users = context.Users.Include(x => x.UserRoles).Where(x => x.Id != session.CurrentLoggedInUser.Id)
                .ToList();
        }
        public ICollection<User> Users { get; set; }
        [Reactive] public User SelectedUser { get; set; }
        [Reactive] public UserRole SelectedUserRole { get; set; }

        private DelegateCommand deleteUser;

        public DelegateCommand DeleteUser => deleteUser ??= new(() =>
        {
            if (MessageBox.Show($"Вы действительно хотите удалить сотрудника {SelectedUser.FullName}?", "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;
            Context db = new Context();
            User deletingUser = db.Users.Single(x => x.Id == SelectedUser.Id);
            db.Users.Remove(deletingUser);
            db.SaveChanges();
            Users.Remove(SelectedUser);
            OnUsersChanged?.Invoke();

        }, () => SelectedUser != null && SelectedUser.Id != session.CurrentLoggedInUser.Id);


        private DelegateCommand addUser;

        public DelegateCommand AddUser => addUser ??= new(() =>
        {

        });

        private DelegateCommand addRoleToUser;

        public DelegateCommand AddRoleToUser => addRoleToUser ??= new(() =>
        {


            OnUserRolesChanged?.Invoke();
        }, () => SelectedUser != null && SelectedUser.Id != session.CurrentLoggedInUser.Id);

        private DelegateCommand revokeRoleFromUser;

        public DelegateCommand RevokeRoleFromUser => revokeRoleFromUser ??= new(() =>
        {
            if (MessageBox.Show($"Вы действительно хотите отозвать роль {SelectedUserRole.Name} у сотрудника {SelectedUser.FullName}?", "Отзыв роли", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
                MessageBoxResult.Yes) return;
            Context db = new();
            UserRole userRole = db.UserRoles.Single(x => x.Id == SelectedUserRole.Id);
            db.UserRoles.Remove(userRole);
            db.SaveChanges();
            SelectedUser.UserRoles.Remove(SelectedUserRole);
            OnUserRolesChanged?.Invoke();
        }, () => SelectedUserRole != null);

        public event Action OnUserRolesChanged;
        public event Action OnUsersChanged;

    }
}
