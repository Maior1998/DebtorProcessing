using System.Windows.Controls;
using System.Windows.Data;

using DebtorsProcessing.Desktop.ViewModel;

namespace DebtorsProcessing.Desktop.View.Pages
{
    /// <summary>
    ///     Interaction logic for AdminPanelUsersManagementView.xaml
    /// </summary>
    public partial class AdminPanelUsersManagementView : Page
    {
        private readonly AdminPanelUsersManagementViewModel viewModel;

        public AdminPanelUsersManagementView()
        {
            InitializeComponent();
            viewModel = (AdminPanelUsersManagementViewModel)DataContext;
            viewModel.OnUserRolesChanged += ViewModel_OnUserRolesChanged;
            viewModel.OnUsersChanged += ViewModel_OnUsersChanged;
        }

        private void ViewModel_OnUsersChanged()
        {
            CollectionViewSource.GetDefaultView(dgUsers.ItemsSource).Refresh();
        }

        private void ViewModel_OnUserRolesChanged()
        {
            CollectionViewSource.GetDefaultView(dgRoles.ItemsSource).Refresh();
        }
    }
}