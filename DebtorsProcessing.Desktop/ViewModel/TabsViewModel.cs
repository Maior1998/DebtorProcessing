using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;
using DebtorsProcessing.Desktop.Services;
using DebtorsProcessing.Desktop.View.Pages;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;

using System;
using System.Windows;
using DebtorsProcessing.Desktop.Model;

namespace DebtorsProcessing.Desktop.ViewModel
{
    public class TabsViewModel : ReactiveObject
    {
        private PageService pageService;
        public TabsViewModel(
            SessionService session,
            PageService pageService)
        {
            Session = session;
            this.pageService = pageService;
        }

        public SessionService Session { get; }

        private AsyncCommand exit;
        public AsyncCommand Exit => exit ??= new(async () =>
        {
            await ServiceTalker.DropCurrentSession();
            Console.WriteLine();
            Session.UserId = Guid.Empty;
            if (MessageBox.Show("Сохранить сеанс для последующего входа?", "Выход из программы", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
            }
            Session.UserSession = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                pageService.NavigateWithoutHistoryCommand.Execute(new LoginView());
            });

        });
    }
}