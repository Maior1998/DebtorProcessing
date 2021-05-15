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

namespace DebtorProcessing.View.Windows
{
    /// <summary>
    /// Interaction logic for ChooseRoleObjectAccessWindow.xaml
    /// </summary>
    public partial class ChooseRoleObjectAccessWindow : Window
    {
        public ChooseRoleObjectAccessWindow()
        {
            InitializeComponent();
            viewModel = (ChooseRoleObjectAccessWindowViewModel)DataContext;
            viewModel.OnSaved += ViewModel_OnSaved;
        }

        public new bool? ShowDialog()
        {
            viewModel.Search(ExcludingObjects);
            return base.ShowDialog();
        }
        public Guid[] ExcludingObjects { get; set; }
        public SecurityObject SelectedSecurityObject { get; set; }
        private void ViewModel_OnSaved(SecurityObject obj)
        {
            SelectedSecurityObject = obj;
            DialogResult = true;
            Close();
        }

        private readonly ChooseRoleObjectAccessWindowViewModel viewModel;

        private void ChooseRoleObjectAccessWindow_OnClosed(object? sender, EventArgs e)
        {
            viewModel.OnSaved -= ViewModel_OnSaved;
        }
    }
}
