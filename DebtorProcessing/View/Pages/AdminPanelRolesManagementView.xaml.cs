using System.Windows.Controls;
using System.Windows.Data;
using DebtorProcessing.ViewModel;

namespace DebtorProcessing.View.Pages
{
    /// <summary>
    /// Interaction logic for AdminPanelRolesManagementView.xaml
    /// </summary>
    public partial class AdminPanelRolesManagementView : Page
    {
        public AdminPanelRolesManagementView()
        {
            InitializeComponent();
            viewModel = (AdminPanelRolesManagementViewModel)DataContext;
            viewModel.OnRolesChanged += ViewModel_OnRolesChanged;
            viewModel.OnObjectsChanged += ViewModelOnObjectsChanged;
        }

        private void ViewModelOnObjectsChanged()
        {
            CollectionViewSource.GetDefaultView(dgObjects.ItemsSource).Refresh();
        }

        private void ViewModel_OnRolesChanged()
        {
            CollectionViewSource.GetDefaultView(dgRoles.ItemsSource).Refresh();
        }

        private AdminPanelRolesManagementViewModel viewModel;

    }
}
