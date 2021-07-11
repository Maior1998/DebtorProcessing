using System.Windows.Controls;
using System.Windows.Input;

using DebtorsProcessing.Desktop.ViewModel;

namespace DebtorsProcessing.Desktop.View.Pages
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
            viewModel = (DebtorsTableViewModel)DataContext;
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (viewModel.EditDebtor.CanExecute(null))
                viewModel.EditDebtor.Execute(null);
        }
    }
}