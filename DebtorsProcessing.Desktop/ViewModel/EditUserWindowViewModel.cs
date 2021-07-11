using System;

using DevExpress.Mvvm;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class EditUserWindowViewModel : ReactiveObject
    {
        private DelegateCommand saveCommand;
        [Reactive] public string FullName { get; set; }
        [Reactive] public string Login { get; set; }

        public DelegateCommand SaveCommand => saveCommand ??= new(() => { OnSaved?.Invoke(); },
            () => !string.IsNullOrWhiteSpace(FullName) && !string.IsNullOrWhiteSpace(Login));

        public event Action OnSaved;
    }
}