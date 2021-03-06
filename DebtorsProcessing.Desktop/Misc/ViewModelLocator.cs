using DebtorsProcessing.Desktop.ViewModel;


namespace DebtorsProcessing.Desktop.Misc
{
    public class ViewModelLocator
    {
        public ChooseSessionViewModel ChooseSessionViewModel => App.Resolve<ChooseSessionViewModel>();
        public EditRoleWindowViewModel EditRoleWindowViewModel => App.Resolve<EditRoleWindowViewModel>();

        public ChooseRoleObjectAccessWindowViewModel ChooseRoleObjectAccessWindowViewModel =>
            App.Resolve<ChooseRoleObjectAccessWindowViewModel>();

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

        public AdminPanelRolesManagementViewModel AdminPanelRolesManagementViewModel =>
            App.Resolve<AdminPanelRolesManagementViewModel>();

        public SettingsViewModel SettingsViewModel => App.Resolve<SettingsViewModel>();

        public ChangePasswordWindowViewModel ChangePasswordWindowViewModel =>
            App.Resolve<ChangePasswordWindowViewModel>();
    }
}