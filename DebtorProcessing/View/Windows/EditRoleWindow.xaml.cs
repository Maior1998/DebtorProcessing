using System;
using System.Windows;
using DebtorProcessing.ViewModel;

namespace DebtorProcessing.View.Windows
{
    /// <summary>
    ///     Interaction logic for EditRoleWindow.xaml
    /// </summary>
    public partial class EditRoleWindow : Window
    {
        private readonly EditRoleWindowViewModel viewModel;

        public EditRoleWindow()
        {
            InitializeComponent();
            viewModel = (EditRoleWindowViewModel) DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }

        public string RoleName { get; set; }

        private void ViewModel_OnSaved()
        {
            RoleName = viewModel.RoleName;
            DialogResult = true;
            Close();
        }

        public new bool? ShowDialog()
        {
            viewModel.RoleName = RoleName;
            return base.ShowDialog();
        }

        private void EditRoleWindow_OnClosed(object sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
        }
    }
}