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
    public class EditRoleWindowViewModel : ReactiveObject
    {

        [Reactive] public string RoleName { get; set; }
        private DelegateCommand save;
        public DelegateCommand Save => save ??= new(() =>
        {
            OnSaved?.Invoke();
        }, () => !string.IsNullOrWhiteSpace(RoleName));

        public event Action OnSaved;
    }
}
