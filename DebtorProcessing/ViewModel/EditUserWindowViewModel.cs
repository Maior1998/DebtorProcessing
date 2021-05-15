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
    public class EditUserWindowViewModel : ReactiveObject
    {
        [Reactive] public string FullName { get; set; }
        [Reactive] public string Login { get; set; }

        public event Action OnSaved;

        private DelegateCommand saveCommand;
        public DelegateCommand SaveCommand => saveCommand ??= new(() =>
        {
            OnSaved?.Invoke();
        }, () => !string.IsNullOrWhiteSpace(FullName) && !string.IsNullOrWhiteSpace(Login));
    }
}
