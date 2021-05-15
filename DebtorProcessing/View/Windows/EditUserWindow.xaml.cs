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
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        public EditUserWindow()
        {
            InitializeComponent();
            viewModel = (EditUserWindowViewModel)DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }

        public new bool? ShowDialog()
        {
            viewModel.Login = Login;
            viewModel.FullName = FullName;
            pbPass.Password = Password;
            if (!IsPassordEditModeEnabled) pbPass.Visibility = Visibility.Collapsed;
            return base.ShowDialog();
        }

        public string Password { get; set; } = string.Empty;
        public string Login { get; set; }
        public string FullName { get; set; }
        public bool IsPassordEditModeEnabled { get; set; }
        private void ViewModel_OnSaved()
        {
            Login = viewModel.Login;
            FullName = viewModel.FullName;
            DialogResult = true;
            Close();
        }

        private readonly EditUserWindowViewModel viewModel;

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
