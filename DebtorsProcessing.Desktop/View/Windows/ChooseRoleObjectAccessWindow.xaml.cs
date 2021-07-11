using System;
using System.Windows;

using DebtorsProcessing.DatabaseModel.Entities;
using DebtorsProcessing.Desktop.ViewModel;

namespace DebtorsProcessing.Desktop.View.Windows
{
    /// <summary>
    ///     Interaction logic for ChooseRoleObjectAccessWindow.xaml
    /// </summary>
    public partial class ChooseRoleObjectAccessWindow : Window
    {
        private readonly ChooseRoleObjectAccessWindowViewModel viewModel;

        public ChooseRoleObjectAccessWindow()
        {
            InitializeComponent();
            viewModel = (ChooseRoleObjectAccessWindowViewModel)DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }

        public Guid[] ExcludingObjects { get; set; }
        public SecurityObject SelectedSecurityObject { get; set; }

        public new bool? ShowDialog()
        {
            viewModel.Search(ExcludingObjects);
            return base.ShowDialog();
        }

        private void ViewModel_OnSaved(SecurityObject obj)
        {
            SelectedSecurityObject = obj;
            DialogResult = true;
            Close();
        }

        private void ChooseRoleObjectAccessWindow_OnClosed(object sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
        }
    }
}