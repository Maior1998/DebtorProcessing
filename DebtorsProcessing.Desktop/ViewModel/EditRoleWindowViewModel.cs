using System;

using DevExpress.Mvvm;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class EditRoleWindowViewModel : ReactiveObject
    {
        private DelegateCommand save;

        [Reactive] public string RoleName { get; set; }

        public DelegateCommand Save =>
            save ??= new(() => { OnSaved?.Invoke(); }, () => !string.IsNullOrWhiteSpace(RoleName));

        public event Action OnSaved;
    }
}