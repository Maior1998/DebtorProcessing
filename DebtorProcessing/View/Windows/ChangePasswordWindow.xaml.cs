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

namespace DebtorProcessing.View.Windows
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
            viewModel = (ChangePasswordWindowViewModel)DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }
        public string NewPassword { get; private set; }
        private void ViewModel_OnSaved()
        {
            NewPassword = viewModel.NewPassword;
            DialogResult = true;
            Close();
        }

        private readonly ChangePasswordWindowViewModel viewModel;

        private void PbNewPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModel.NewPassword = ((PasswordBox)sender).Password;
        }

        private void PbConfirmNewPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModel.ConfirmPassword = ((PasswordBox)sender).Password;
        }

        private void ChangePasswordWindow_OnClosed(object? sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
        }
    }
}
