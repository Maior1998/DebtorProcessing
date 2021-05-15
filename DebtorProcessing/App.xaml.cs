using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

using DebtorProcessing.Services;
using DebtorProcessing.View;
using DebtorProcessing.ViewModel;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using Microsoft.Extensions.DependencyInjection;

using OfficeOpenXml;

namespace DebtorProcessing
{
    /// <summary>
    /// Interaction logic for App.xaml
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
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));


            ServiceCollection serviceCollection = new();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static T Resolve<T>() => ServiceProvider.GetRequiredService<T>();

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
            services.AddTransient<EditRoleWindowViewModel>();
            services.AddTransient<PaymentEditWindowViewModel>();
            services.AddTransient<EditUserWindowViewModel>();
            services.AddTransient<ChooseUserRoleWindowViewModel>();
            services.AddTransient<ChooseRoleObjectAccessWindowViewModel>();
            Context model = new();
            Context.CreateTestData(model);
        }
    }
}
