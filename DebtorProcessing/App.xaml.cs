using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using DebtorProcessing.Services;
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
            services.AddSingleton<DebtorsTableViewModel>();







            DbModel model = new();
            DbModel.CreateTestData(model);
        }
    }
}
