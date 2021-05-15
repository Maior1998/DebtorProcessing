using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class ChooseUserRoleWindowViewModel : ReactiveObject
    {
        public void Search(Guid[] excludingGuids)
        {
            Context db = new();
            if (excludingGuids == null || excludingGuids.Length == 0)
            {
                Roles = db.UserRoles
                    .OrderBy(x => x.Name)
                    .Select(x => new UserRoleSelectingItem() { Role = x })
                    .ToArray();
            }
            else
            {
                Roles = db.UserRoles
                    .OrderBy(x => x.Name)
                    .Where(x => !excludingGuids.Contains(x.Id))
                    .Select(x => new UserRoleSelectingItem() { Role = x })
                    .ToArray();
            }

        }


        [Reactive] public UserRoleSelectingItem[] Roles { get; set; }

        public class UserRoleSelectingItem : ReactiveObject
        {
            public UserRole Role { get; set; }
            [Reactive] public bool IsSelected { get; set; }
        }

        private DelegateCommand save;

        public DelegateCommand Save => save ??= new(() =>
        {
            OnSaved?.Invoke(Roles.Single(x => x.IsSelected).Role);
        }, () => Roles != null && Roles.Any(x => x.IsSelected));

        public event Action<UserRole> OnSaved;
    }
}
