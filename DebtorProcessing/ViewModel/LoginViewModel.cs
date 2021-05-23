using System.Linq;
using System.Windows;

using DebtorProcessing.Services;
using DebtorProcessing.View;

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

        private DelegateCommand loginCommand;

        public LoginViewModel(PageService pageService, SessionService sessionService)
        {
            this.pageService = pageService;
            this.sessionService = sessionService;
        }

        [Reactive] public string Login { get; set; }
        [Reactive] public string Password { get; set; }

        public DelegateCommand LoginCommand => loginCommand ??= new(() =>
        {
            string hash = User.GetHashedString(Password);
            Context model = new();

            User user = model.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Objects)
                .SingleOrDefault(x =>
                    x.Login.ToLower() == Login.ToLower()
                    && x.PasswordHash.ToLower() == hash.ToLower());
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            sessionService.CurrentLoggedInUser = user;
            pageService.NavigateCommand.Execute(new TabsView());
        }, () => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password));
    }
}