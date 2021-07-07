using System.Linq;
using System.Windows;

using DebtorProcessing.Services;
using DebtorProcessing.View;
using DebtorProcessing.View.Pages;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class LoginViewModel : ReactiveObject
    {
        private readonly PageService pageService;
        private readonly SessionService sessionService;

        private AsyncCommand loginCommand;

        public LoginViewModel(PageService pageService, SessionService sessionService)
        {
            this.pageService = pageService;
            this.sessionService = sessionService;
        }

        [Reactive] public string Login { get; set; }
        [Reactive] public string Password { get; set; }

        public AsyncCommand LoginCommand => loginCommand ??= new(async () =>
       {
           string hash = User.GetHashedString(Password);
           Context model = new();

           User user = await model.Users
               .SingleOrDefaultAsync(x =>
                   x.Login.ToLower() == Login.ToLower()
                   && x.PasswordHash.ToLower() == hash.ToLower());
           if (user == null)
           {
               MessageBox.Show("Неверный логин или пароль", "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
               return;
           }

           sessionService.UserId = user.Id;
           Application.Current.Dispatcher.Invoke(() => pageService.NavigateCommand.Execute(new ChooseSessionPage()));

       }, () => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password));
    }
}