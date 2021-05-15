using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using DebtorProcessing.ViewModel;

namespace DebtorProcessing.View
{
    /// <summary>
    /// Interaction logic for PaymentEditWindow.xaml
    /// </summary>
    public partial class PaymentEditWindow : Window
    {
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

        private PaymentEditWindowViewModel viewModel;

        private void PaymentEditWindow_OnClosed(object sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
            viewModel = null;
        }
    }
}
