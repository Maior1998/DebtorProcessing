using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DebtorProcessing.ViewModel;

using ReactiveUI;

namespace DebtorProcessing.Misc
{
    public class ViewModelLocator : ReactiveObject
    {
        public EditRoleWindowViewModel EditRoleWindowViewModel => App.Resolve<EditRoleWindowViewModel>();
        public ChooseRoleObjectAccessWindowViewModel ChooseRoleObjectAccessWindowViewModel => App.Resolve<ChooseRoleObjectAccessWindowViewModel>();
        public ChooseUserRoleWindowViewModel ChooseUserRoleWindowViewModel =>
            App.Resolve<ChooseUserRoleWindowViewModel>();
        public MainViewModel MainViewModel => App.Resolve<MainViewModel>();
        public LoginViewModel LoginViewModel => App.Resolve<LoginViewModel>();
        public DebtorsTableViewModel DebtorsTableViewModel => App.Resolve<DebtorsTableViewModel>();
        public DebtorsEditViewModel DebtorsEditViewModel => App.Resolve<DebtorsEditViewModel>();
        public PaymentEditWindowViewModel PaymentEditWindowViewModel => App.Resolve<PaymentEditWindowViewModel>();
        public TabsViewModel TabsViewModel => App.Resolve<TabsViewModel>();
        public AdminPanelViewModel AdminPanelViewModel => App.Resolve<AdminPanelViewModel>();
        public EditUserWindowViewModel EditUserWindowViewModel => App.Resolve<EditUserWindowViewModel>();
        public AdminPanelUsersManagementViewModel AdminPanelUsersManagementViewModel =>
            App.Resolve<AdminPanelUsersManagementViewModel>();
        public AdminPanelRolesManagementViewModel AdminPanelRolesManagementViewModel => App.Resolve<AdminPanelRolesManagementViewModel>();
    }
}
