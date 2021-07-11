using System;
using System.Windows;

using DebtorsProcessing.Desktop.ViewModel;

namespace DebtorsProcessing.Desktop.View.Windows
{
    /// <summary>
    ///     Interaction logic for PaymentEditWindow.xaml
    /// </summary>
    public partial class PaymentEditWindow : Window
    {
        private PaymentEditWindowViewModel viewModel;

        public PaymentEditWindow()
        {
            InitializeComponent();
            viewModel = (PaymentEditWindowViewModel)DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }

        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public decimal PaymentAmount { get; set; }

        public new bool? ShowDialog()
        {
            viewModel.PaymentAmount = PaymentAmount;
            viewModel.PaymentDate = PaymentDate;
            return base.ShowDialog();
        }

        private void ViewModel_OnSaved()
        {
            PaymentAmount = viewModel.PaymentAmount;
            PaymentDate = viewModel.PaymentDate;
            DialogResult = true;
            Close();
        }

        private void PaymentEditWindow_OnClosed(object sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
            viewModel = null;
        }
    }
}