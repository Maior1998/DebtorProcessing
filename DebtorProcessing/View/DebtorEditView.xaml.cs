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
using System.Windows.Navigation;
using System.Windows.Shapes;

using DebtorProcessing.ViewModel;

namespace DebtorProcessing.View
{
    /// <summary>
    /// Interaction logic for DebtorEditView.xaml
    /// </summary>
    public partial class DebtorEditView : Page
    {
        public DebtorEditView(Guid debtorId)
        {
            InitializeComponent();
            viewModel = (DebtorsEditViewModel)DataContext;
            viewModel.OnPaymentsChanged += ViewModel_OnPaymentsChanged;
            viewModel.SetDebtor(debtorId);
        }

        private void ViewModel_OnPaymentsChanged()
        {
            CollectionViewSource.GetDefaultView(dgPayments.ItemsSource).Refresh();
        }

        private DebtorsEditViewModel viewModel;
    }
}
