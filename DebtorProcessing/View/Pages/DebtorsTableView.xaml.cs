using System.Windows.Controls;
using System.Windows.Input;
using DebtorProcessing.ViewModel;

namespace DebtorProcessing.View
{
    /// <summary>
    ///     Interaction logic for DebtorsTableView.xaml
    /// </summary>
    public partial class DebtorsTableView : Page
    {
        private readonly DebtorsTableViewModel viewModel;

        public DebtorsTableView()
        {
            InitializeComponent();
            viewModel = (DebtorsTableViewModel) DataContext;
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (viewModel.EditDebtor.CanExecute(null))
                viewModel.EditDebtor.Execute(null);
        }
    }
}