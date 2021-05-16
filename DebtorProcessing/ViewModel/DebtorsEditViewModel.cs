using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DebtorProcessing.Services;
using DebtorProcessing.View;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class DebtorsEditViewModel : ReactiveObject
    {

        public PageService PageService { get; set; }
        public SessionService session;
        public DebtorsEditViewModel(PageService pageService, SessionService session)
        {
            PageService = pageService;
            this.session = session;
        }
        public void SetDebtor(Guid id)
        {
            trackingContext = new();
            Debtor = trackingContext.Debtors
                .Include(x => x.Payments)
                .Single(x => x.Id == id);
        }

        private Context trackingContext;

        private DelegateCommand backCommand;
        public DelegateCommand BackCommand => backCommand ??= new(GoBack);

        [Reactive] public Debtor Debtor { get; set; }
        [Reactive] public DebtorPayment SelectedPayment { get; set; }

        private DelegateCommand save;

        private void GoBack()
        {
            PageService.BackCommand.Execute(null);
        }

        public DelegateCommand Save => save ??= new(() =>
        {
            trackingContext.SaveChanges();
            GoBack();
        }, () => session.CanEditNotOwnedDebtorsData || (Debtor.Responsible != null && Debtor.Responsible.Id == session.CurrentLoggedInUser.Id));

        private DelegateCommand addPayment;
        public DelegateCommand AddPayment => addPayment ??= new(() =>
        {
            PaymentEditWindow paymentEditWindow = new();
            if (!paymentEditWindow.ShowDialog().Value) return;
            DebtorPayment payment = new()
            {
                Date = paymentEditWindow.PaymentDate,
                Amount = paymentEditWindow.PaymentAmount
            };
            Debtor.Payments.Add(payment);
            OnPaymentsChanged?.Invoke();
        });

        private DelegateCommand editPayment;
        public DelegateCommand EditPayment => editPayment ??= new(() =>
        {
            PaymentEditWindow paymentEditWindow = new()
            {
                PaymentAmount = SelectedPayment.Amount,
                PaymentDate = SelectedPayment.Date
            };
            if (!paymentEditWindow.ShowDialog().Value) return;
            SelectedPayment.Date = paymentEditWindow.PaymentDate;
            SelectedPayment.Amount = paymentEditWindow.PaymentAmount;
            OnPaymentsChanged?.Invoke();
        });

        private DelegateCommand deletePayment;
        public DelegateCommand DeletePayment => deletePayment ??= new(() =>
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить этот платеж?", "Удаление платежа",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            Debtor.Payments.Remove(SelectedPayment);
            OnPaymentsChanged?.Invoke();
        });

        public event Action OnPaymentsChanged;
    }
}
