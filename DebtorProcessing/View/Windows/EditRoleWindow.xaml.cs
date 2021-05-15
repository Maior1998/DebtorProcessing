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
    /// Interaction logic for EditRoleWindow.xaml
    /// </summary>
    public partial class EditRoleWindow : Window
    {
        public EditRoleWindow()
        {
            InitializeComponent();
            viewModel = (EditRoleWindowViewModel)DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }

        private void ViewModel_OnSaved()
        {
            RoleName = viewModel.RoleName;
            DialogResult = true;
            Close();
        }

        private readonly EditRoleWindowViewModel viewModel;
        public string RoleName { get; set; }

        public new bool? ShowDialog()
        {
            viewModel.RoleName = RoleName;
            return base.ShowDialog();
        }

        private void EditRoleWindow_OnClosed(object? sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
        }
    }
}
