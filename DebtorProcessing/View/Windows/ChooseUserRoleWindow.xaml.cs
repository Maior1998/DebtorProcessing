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

using DebtorsDbModel.Model;

namespace DebtorProcessing.View
{
    /// <summary>
    /// Interaction logic for ChooseUserRoleWindow.xaml
    /// </summary>
    public partial class ChooseUserRoleWindow : Window
    {
        public ChooseUserRoleWindow()
        {
            InitializeComponent();
            viewModel = (ChooseUserRoleWindowViewModel)DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }

        public new bool? ShowDialog()
        {
            viewModel.Search(ExcludingRoles);
            return base.ShowDialog();
        }
        public Guid[] ExcludingRoles { get; set; }
        public UserRole SelectedUserRole { get; set; }
        private void ViewModel_OnSaved(DebtorsDbModel.Model.UserRole obj)
        {
            SelectedUserRole = obj;
            DialogResult = true;
            Close();
        }

        private readonly ChooseUserRoleWindowViewModel viewModel;

        private void ChooseUserRoleWindow_OnClosed(object? sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
        }
    }
}
