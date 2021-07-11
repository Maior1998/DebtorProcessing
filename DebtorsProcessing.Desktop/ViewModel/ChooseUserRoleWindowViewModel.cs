using System;
using System.Linq;

using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using DevExpress.Mvvm;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class ChooseUserRoleWindowViewModel : ReactiveObject
    {
        private DelegateCommand save;


        [Reactive] public UserRoleSelectingItem[] Roles { get; set; }

        public DelegateCommand Save => save ??= new(() => { OnSaved?.Invoke(Roles.Single(x => x.IsSelected).Role); },
            () => Roles != null && Roles.Any(x => x.IsSelected));

        public void Search(Guid[] excludingGuids)
        {
            DebtorsContext db = new();
            if (excludingGuids == null || excludingGuids.Length == 0)
                Roles = db.UserRoles
                    .OrderBy(x => x.Name)
                    .Select(x => new UserRoleSelectingItem { Role = x })
                    .ToArray();
            else
                Roles = db.UserRoles
                    .OrderBy(x => x.Name)
                    .Where(x => !excludingGuids.Contains(x.Id))
                    .Select(x => new UserRoleSelectingItem { Role = x })
                    .ToArray();
        }

        public event Action<UserRole> OnSaved;

        public class UserRoleSelectingItem : ReactiveObject
        {
            public UserRole Role { get; set; }
            [Reactive] public bool IsSelected { get; set; }
        }
    }
}