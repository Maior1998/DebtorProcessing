using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;
using DebtorsProcessing.Desktop.View.Windows;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class AdminPanelRolesManagementViewModel : ReactiveObject
    {
        private DelegateCommand addRole;

        private DelegateCommand addRoleObjectAccess;
        private Guid[] availiableObjectsToUse = new Guid[0];

        private DelegateCommand deleteRole;


        private DelegateCommand revokeSecurityObjectAccess;

        public AdminPanelRolesManagementViewModel()
        {
            UpdateRoles();
            UpdateAvailiableObjects();
        }

        [Reactive] public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
        [Reactive] public UserRole SelectedRole { get; set; }
        [Reactive] public SecurityObject SelectedSecurityObject { get; set; }

        public DelegateCommand AddRole => addRole ??= new(() =>
        {
            EditRoleWindow editRoleWindow = new()
            {
                RoleName = "Новая роль"
            };
            if (!editRoleWindow.ShowDialog().Value) return;
            DebtorsContext db = new();
            UserRole duplicateRole =
                db.UserRoles.SingleOrDefault(x => x.Name.ToLower() == editRoleWindow.RoleName.ToLower());
            if (duplicateRole != null)
            {
                MessageBox.Show("Роль с таким именем уже существует!", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            UserRole addingRole = new() { Name = editRoleWindow.RoleName };
            db.UserRoles.Add(addingRole);
            db.SaveChanges();
            Roles.Add(addingRole);
            OnRolesChanged?.Invoke();
        });

        public DelegateCommand DeleteRole => deleteRole ??= new(() =>
        {
            if (MessageBox.Show($"Вы действительно хотите удалить роль{SelectedRole.Name}?", "Удаление роли",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            DebtorsContext db = new();
            UserRole userRole = db.UserRoles.Single(x => x.Id == SelectedRole.Id);
            db.UserRoles.Remove(userRole);
            db.SaveChanges();
        }, () => SelectedRole != null);

        public DelegateCommand AddRoleObjectAccess => addRoleObjectAccess ??= new(() =>
        {
            ChooseRoleObjectAccessWindow chooseRoleObjectAccessWindow = new()
            {
                ExcludingObjects = SelectedRole.Objects.Select(x => x.Id).ToArray()
            };
            if (!chooseRoleObjectAccessWindow.ShowDialog().Value) return;
            DebtorsContext db = new();
            UserRole role = db.UserRoles.Single(x => x.Id == SelectedRole.Id);
            role.Objects.Add(db.SecurityObjects.Single(x =>
                x.Id == chooseRoleObjectAccessWindow.SelectedSecurityObject.Id));
            db.SaveChanges();
            SelectedRole.Objects.Add(chooseRoleObjectAccessWindow.SelectedSecurityObject);
            OnObjectsChanged?.Invoke();
        }, () => SelectedRole != null && availiableObjectsToUse.Except(SelectedRole.Objects.Select(x => x.Id)).Any());

        public DelegateCommand RevokeSecurityObjectAccess => revokeSecurityObjectAccess ??= new(() =>
        {
            if (MessageBox.Show(
                $"Вы действительно хотите отозвать у роли {SelectedRole.Name} право {SelectedSecurityObject.Name}?",
                "Удаление роли", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            DebtorsContext db = new();
            SecurityObject securityObject = db.SecurityObjects.Single(x => x.Id == SelectedSecurityObject.Id);
            UserRole role = db.UserRoles
                .Include(x => x.Objects)
                .Single(x => x.Id == SelectedRole.Id);
            role.Objects.Remove(securityObject);
            SelectedRole.Objects.Remove(SelectedSecurityObject);
            OnObjectsChanged?.Invoke();
        }, () => SelectedSecurityObject != null);

        private void UpdateRoles()
        {
            DebtorsContext db = new();
            Roles = db.UserRoles
                .Include(x => x.Objects)
                .ToArray();
        }

        private void UpdateAvailiableObjects()
        {
            DebtorsContext db = new();
            availiableObjectsToUse = db.SecurityObjects.Select(x => x.Id).ToArray();
        }

        public event Action OnRolesChanged;
        public event Action OnObjectsChanged;
    }
}