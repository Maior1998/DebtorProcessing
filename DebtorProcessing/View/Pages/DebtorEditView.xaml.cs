using System;
using System.Windows.Controls;
using System.Windows.Data;
using DebtorProcessing.ViewModel;

namespace DebtorProcessing.View
{
    /// <summary>
    ///     Interaction logic for DebtorEditView.xaml
    /// </summary>
    public partial class DebtorEditView : Page
    {
        private readonly DebtorsEditViewModel viewModel;

        public DebtorEditView(Guid debtorId)
        {
            InitializeComponent();
            viewModel = (DebtorsEditViewModel) DataContext;
            viewModel.OnPaymentsChanged += ViewModel_OnPaymentsChanged;
            viewModel.SetDebtor(debtorId);
        }

        private void ViewModel_OnPaymentsChanged()
        {
            CollectionViewSource.GetDefaultView(dgPayments.ItemsSource).Refresh();
        }
    }
}