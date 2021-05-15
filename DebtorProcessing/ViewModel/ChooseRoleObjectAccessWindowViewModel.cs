using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class ChooseRoleObjectAccessWindowViewModel : ReactiveObject
    {
        public void Search(Guid[] excludingGuids)
        {
            Context db = new();
            if (excludingGuids == null || excludingGuids.Length == 0)
            {
                SecurityObjects = db.SecurityObjects
                    .OrderBy(x => x.Name)
                    .Select(x => new SecurityObjectSelectingItem() { SecurityObject = x })
                    .ToArray();
            }
            else
            {
                SecurityObjects = db.SecurityObjects
                    .OrderBy(x => x.Name)
                    .Where(x => !excludingGuids.Contains(x.Id))
                    .Select(x => new SecurityObjectSelectingItem() { SecurityObject = x })
                    .ToArray();
            }

        }


        [Reactive] public SecurityObjectSelectingItem[] SecurityObjects { get; set; }

        public class SecurityObjectSelectingItem : ReactiveObject
        {
            public SecurityObject SecurityObject { get; set; }
            [Reactive] public bool IsSelected { get; set; }
        }

        private DelegateCommand save;

        public DelegateCommand Save => save ??= new(() =>
        {
            OnSaved?.Invoke(SecurityObjects.Single(x => x.IsSelected).SecurityObject);
        }, () => SecurityObjects != null && SecurityObjects.Any(x => x.IsSelected));

        public event Action<SecurityObject> OnSaved;
    }
}
