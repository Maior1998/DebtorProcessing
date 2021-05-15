using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DebtorProcessing.View.Windows;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class AdminPanelRolesManagementViewModel : ReactiveObject
    {
        [Reactive] public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
        [Reactive] public UserRole SelectedRole { get; set; }
        [Reactive] public RoleObjectAccess SelectedRoleObjectAccess { get; set; }
        private Guid[] availiableObjectsToUse = new Guid[0];

        public AdminPanelRolesManagementViewModel()
        {
            UpdateRoles();
            UpdateAvailiableObjects();
        }

        private void UpdateRoles()
        {
            Context db = new Context();
            Roles = db.UserRoles
                .Include(x => x.RoleObjectAccesses)
                .ThenInclude(x => x.Object)
                .ToArray();
        }
        private void UpdateAvailiableObjects()
        {
            Context db = new();
            availiableObjectsToUse = db.SecurityObjects.Select(x => x.Id).ToArray();
        }


        private DelegateCommand addRole;
        public DelegateCommand AddRole => addRole ??= new(() =>
        {
            EditRoleWindow editRoleWindow = new EditRoleWindow()
            {
                RoleName = "Новая роль"
            };
            if (!editRoleWindow.ShowDialog().Value) return;
            Context db = new Context();
            UserRole duplicateRole =
                db.UserRoles.SingleOrDefault(x => x.Name.ToLower() == editRoleWindow.RoleName.ToLower());
            if (duplicateRole != null)
            {
                MessageBox.Show("Роль с таким именем уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UserRole addingRole = new() { Name = editRoleWindow.RoleName };
            db.UserRoles.Add(addingRole);
            db.SaveChanges();
            Roles.Add(addingRole);
            OnRolesChanged?.Invoke();
        });

        private DelegateCommand deleteRole;

        public DelegateCommand DeleteRole => deleteRole ??= new DelegateCommand(() =>
        {
            if (MessageBox.Show($"Вы действительно хотите удалить роль{SelectedRole.Name}?", "Удаление роли", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            Context db = new();
            UserRole userRole = db.UserRoles.Single(x => x.Id == SelectedRole.Id);
            db.UserRoles.Remove(userRole);
            db.SaveChanges();

        }, () => SelectedRole != null);

        private DelegateCommand addRoleObjectAccess;
        public DelegateCommand AddRoleObjectAccess => addRoleObjectAccess ??= new(() =>
        {
            ChooseRoleObjectAccessWindow chooseRoleObjectAccessWindow = new()
            {
                ExcludingObjects = SelectedRole.RoleObjectAccesses.Select(x => x.ObjectId).ToArray()
            };
            if (!chooseRoleObjectAccessWindow.ShowDialog().Value) return;
            Context db = new Context();
            RoleObjectAccess roleObjectAccess = new()
            {
                ObjectId = chooseRoleObjectAccessWindow.SelectedSecurityObject.Id,
                UserRoleId = SelectedRole.Id
            };
            db.RoleObjectAccesses.Add(roleObjectAccess);
            db.SaveChanges();
            SelectedRole.RoleObjectAccesses.Add(new()
            {
                Id = roleObjectAccess.Id,
                Object = chooseRoleObjectAccessWindow.SelectedSecurityObject,
                ObjectId = chooseRoleObjectAccessWindow.SelectedSecurityObject.Id
            });
            OnObjectsChanged?.Invoke();

        }, () => SelectedRole != null && availiableObjectsToUse.Except(SelectedRole.RoleObjectAccesses.Select(x => x.ObjectId)).Any());


        private DelegateCommand revokeRoleObjectAccess;
        public DelegateCommand RevokeRoleObjectAccess => revokeRoleObjectAccess ??= new DelegateCommand(() =>
        {
            if (MessageBox.Show($"Вы действительно хотите отозвать у роли {SelectedRole.Name} право {SelectedRoleObjectAccess.Object.Name}?", "Удаление роли", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            Context db = new Context();
            RoleObjectAccess roleObjectAccess = db.RoleObjectAccesses.Single(x => x.Id == SelectedRoleObjectAccess.Id);
            db.RoleObjectAccesses.Remove(roleObjectAccess);
            db.SaveChanges();
            SelectedRole.RoleObjectAccesses.Remove(SelectedRoleObjectAccess);
            OnObjectsChanged?.Invoke();
        }, () => SelectedRoleObjectAccess != null);

        public event Action OnRolesChanged;
        public event Action OnObjectsChanged;
    }
}
