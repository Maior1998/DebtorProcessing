using System;
using DevExpress.Mvvm;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class ChangePasswordWindowViewModel : ReactiveObject
    {
        private DelegateCommand save;
        [Reactive] public string NewPassword { get; set; }
        [Reactive] public string ConfirmPassword { get; set; }

        public DelegateCommand Save => save ??= new(() => { OnSaved?.Invoke(); },
            () => !string.IsNullOrWhiteSpace(NewPassword) && NewPassword == ConfirmPassword);

        public event Action OnSaved;
    }
}