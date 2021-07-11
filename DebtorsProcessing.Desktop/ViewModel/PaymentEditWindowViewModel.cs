using System;

using DevExpress.Mvvm;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class PaymentEditWindowViewModel : ReactiveObject
    {
        private DelegateCommand saveCommand;
        [Reactive] public DateTime PaymentDate { get; set; }
        [Reactive] public decimal PaymentAmount { get; set; }

        public DelegateCommand SaveCommand => saveCommand ??= new(() => { OnSaved?.Invoke(); });

        public event Action OnSaved;
    }
}