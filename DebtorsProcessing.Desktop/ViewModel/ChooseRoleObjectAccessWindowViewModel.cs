using System;
using System.Linq;

using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using DevExpress.Mvvm;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class ChooseRoleObjectAccessWindowViewModel : ReactiveObject
    {
        private DelegateCommand save;


        [Reactive] public SecurityObjectSelectingItem[] SecurityObjects { get; set; }

        public DelegateCommand Save => save ??=
            new(() => { OnSaved?.Invoke(SecurityObjects.Single(x => x.IsSelected).SecurityObject); },
                () => SecurityObjects != null && SecurityObjects.Any(x => x.IsSelected));

        public void Search(Guid[] excludingGuids)
        {
            DebtorsContext db = new();
            if (excludingGuids == null || excludingGuids.Length == 0)
                SecurityObjects = db.SecurityObjects
                    .OrderBy(x => x.Name)
                    .Select(x => new SecurityObjectSelectingItem { SecurityObject = x })
                    .ToArray();
            else
                SecurityObjects = db.SecurityObjects
                    .OrderBy(x => x.Name)
                    .Where(x => !excludingGuids.Contains(x.Id))
                    .Select(x => new SecurityObjectSelectingItem { SecurityObject = x })
                    .ToArray();
        }

        public event Action<SecurityObject> OnSaved;

        public class SecurityObjectSelectingItem : ReactiveObject
        {
            public SecurityObject SecurityObject { get; set; }
            [Reactive] public bool IsSelected { get; set; }
        }
    }
}