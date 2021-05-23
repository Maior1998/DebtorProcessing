using System;
using System.Windows;
using DebtorProcessing.ViewModel;
using DebtorsDbModel.Model;

namespace DebtorProcessing.View
{
    /// <summary>
    ///     Interaction logic for ChooseUserRoleWindow.xaml
    /// </summary>
    public partial class ChooseUserRoleWindow : Window
    {
        private readonly ChooseUserRoleWindowViewModel viewModel;

        public ChooseUserRoleWindow()
        {
            InitializeComponent();
            viewModel = (ChooseUserRoleWindowViewModel) DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }

        public Guid[] ExcludingRoles { get; set; }
        public UserRole SelectedUserRole { get; set; }

        public new bool? ShowDialog()
        {
            viewModel.Search(ExcludingRoles);
            return base.ShowDialog();
        }

        private void ViewModel_OnSaved(UserRole obj)
        {
            SelectedUserRole = obj;
            DialogResult = true;
            Close();
        }

        private void ChooseUserRoleWindow_OnClosed(object? sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
        }
    }
}