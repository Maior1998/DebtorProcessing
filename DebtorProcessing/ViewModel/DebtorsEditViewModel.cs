using System;
using System.Linq;
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
        private DelegateCommand addPayment;

        private DelegateCommand backCommand;

        private DelegateCommand deletePayment;

        private DelegateCommand editPayment;

        private DelegateCommand save;
        public SessionService session;

        private Context trackingContext;

        public DebtorsEditViewModel(PageService pageService, SessionService session)
        {
            PageService = pageService;
            this.session = session;
        }

        public PageService PageService { get; set; }
        public DelegateCommand BackCommand => backCommand ??= new(GoBack);

        [Reactive] public Debtor Debtor { get; set; }
        [Reactive] public DebtorPayment SelectedPayment { get; set; }

        public DelegateCommand Save => save ??= new(() =>
            {
                trackingContext.SaveChanges();
                GoBack();
            },
            () => session.CanEditNotOwnedDebtorsData ||
                  Debtor.Responsible != null && Debtor.Responsible.Id == session.CurrentLoggedInUser.Id);

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

        public DelegateCommand DeletePayment => deletePayment ??= new(() =>
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить этот платеж?", "Удаление платежа",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            Debtor.Payments.Remove(SelectedPayment);
            OnPaymentsChanged?.Invoke();
        });

        public void SetDebtor(Guid id)
        {
            trackingContext = new();
            Debtor = trackingContext.Debtors
                .Include(x => x.Payments)
                .Single(x => x.Id == id);
            OnPaymentsChanged?.Invoke();
        }

        private void GoBack()
        {
            PageService.BackCommand.Execute(null);
        }

        public event Action OnPaymentsChanged;
    }
}