using System;
using System.Windows;
using System.Windows.Controls;
using DebtorProcessing.ViewModel;

namespace DebtorProcessing.View.Windows
{
    /// <summary>
    ///     Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private readonly ChangePasswordWindowViewModel viewModel;

        public ChangePasswordWindow()
        {
            InitializeComponent();
            viewModel = (ChangePasswordWindowViewModel) DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }

        public string NewPassword { get; private set; }

        private void ViewModel_OnSaved()
        {
            NewPassword = viewModel.NewPassword;
            DialogResult = true;
            Close();
        }

        private void PbNewPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModel.NewPassword = ((PasswordBox) sender).Password;
        }

        private void PbConfirmNewPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModel.ConfirmPassword = ((PasswordBox) sender).Password;
        }

        private void ChangePasswordWindow_OnClosed(object sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
        }
    }
}