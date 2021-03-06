using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

using DebtorsProcessing.Desktop.Services;
using DebtorsProcessing.Desktop.ViewModel;

using Microsoft.Extensions.DependencyInjection;

using OfficeOpenXml;

namespace DebtorsProcessing.Desktop
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IServiceProvider ServiceProvider;


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CultureInfo.DefaultThreadCurrentCulture = new("ru-RU");
            Thread.CurrentThread.CurrentCulture = new("ru-RU");
            Thread.CurrentThread.CurrentUICulture = new("ru-RU");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));


            ServiceCollection serviceCollection = new();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static T Resolve<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SessionService>();
            services.AddSingleton<PageService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddTransient<DebtorsTableViewModel>();
            services.AddTransient<DebtorsEditViewModel>();
            services.AddSingleton<TabsViewModel>();
            services.AddSingleton<AdminPanelViewModel>();
            services.AddSingleton<AdminPanelUsersManagementViewModel>();
            services.AddSingleton<AdminPanelRolesManagementViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddTransient<EditRoleWindowViewModel>();
            services.AddTransient<PaymentEditWindowViewModel>();
            services.AddTransient<EditUserWindowViewModel>();
            services.AddTransient<ChooseUserRoleWindowViewModel>();
            services.AddTransient<ChooseRoleObjectAccessWindowViewModel>();
            services.AddTransient<ChangePasswordWindowViewModel>();
            services.AddTransient<ChooseSessionViewModel>();
        }
    }
}