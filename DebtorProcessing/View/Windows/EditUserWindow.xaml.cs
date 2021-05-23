using System;
using System.Windows;
using DebtorProcessing.ViewModel;

namespace DebtorProcessing.View
{
    /// <summary>
    ///     Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private readonly EditUserWindowViewModel viewModel;

        public EditUserWindow()
        {
            InitializeComponent();
            viewModel = (EditUserWindowViewModel) DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }

        public string Password { get; set; } = string.Empty;
        public string Login { get; set; }
        public string FullName { get; set; }
        public bool IsPassordEditModeEnabled { get; set; }

        public new bool? ShowDialog()
        {
            viewModel.Login = Login;
            viewModel.FullName = FullName;
            pbPass.Password = Password;
            if (!IsPassordEditModeEnabled) pbPass.Visibility = Visibility.Collapsed;
            return base.ShowDialog();
        }

        private void ViewModel_OnSaved()
        {
            Login = viewModel.Login;
            FullName = viewModel.FullName;
            DialogResult = true;
            Close();
        }

        private void EditUserWindow_OnClosed(object sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
        }

        private void PbPass_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = pbPass.Password;
        }
    }
}