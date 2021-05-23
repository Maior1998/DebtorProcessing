using System.Windows;
using System.Windows.Controls;
using DebtorProcessing.ViewModel;

namespace DebtorProcessing.View
{
    /// <summary>
    ///     Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        private readonly LoginViewModel _viewModel;

        public LoginView()
        {
            InitializeComponent();
            _viewModel = (LoginViewModel) DataContext;
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = ((PasswordBox) sender).Password;
        }
    }
}