using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.Mvvm;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class ChangePasswordWindowViewModel : ReactiveObject
    {
        [Reactive] public string NewPassword { get; set; }
        [Reactive] public string ConfirmPassword { get; set; }

        private DelegateCommand save;

        public DelegateCommand Save => save ??= new(() =>
        {
            OnSaved?.Invoke();
        }, () => !string.IsNullOrWhiteSpace(NewPassword) && NewPassword == ConfirmPassword);

        public event Action OnSaved;
    }
}
