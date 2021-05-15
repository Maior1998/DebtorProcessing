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
    public class PaymentEditWindowViewModel : ReactiveObject
    {
        [Reactive] public DateTime PaymentDate { get; set; }
        [Reactive] public decimal PaymentAmount { get; set; }

        public event Action OnSaved;

        private DelegateCommand saveCommand;

        public DelegateCommand SaveCommand => saveCommand ??= new DelegateCommand(() =>
        {
            OnSaved?.Invoke();
        });
    }
}
