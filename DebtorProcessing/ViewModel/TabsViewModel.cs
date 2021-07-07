using DebtorProcessing.Services;
using DebtorProcessing.View;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;

using System;
using System.Windows;

namespace DebtorProcessing.ViewModel
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

            Session.UserId = Guid.Empty;
            if (MessageBox.Show("Сохранить сеанс для последующего входа?", "Выход из программы", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                Context db = new Context();
                UserSession session = await db.Sessions.SingleAsync(x => x.Id == Session.UserSession.Id);
                session.EndDate = DateTime.Now;
                await db.SaveChangesAsync();
            }
            Session.UserSession = null;
            App.Current.Dispatcher.Invoke(() => 
            {
                pageService.NavigateWithoutHistoryCommand.Execute(new LoginView());
            });

        });
    }
}