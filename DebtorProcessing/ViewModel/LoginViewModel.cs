using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DebtorProcessing.Services;
using DebtorProcessing.View;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorProcessing.ViewModel
{
    public class LoginViewModel : ReactiveObject
    {
        private readonly PageService pageService;
        private readonly SessionService sessionService;
        public LoginViewModel(PageService pageService, SessionService sessionService)
        {
            this.pageService = pageService;
            this.sessionService = sessionService;
        }

        [Reactive] public string Login { get; set; }
        [Reactive] public string Password { get; set; }

        private DelegateCommand loginCommand;

        public DelegateCommand LoginCommand => loginCommand ??= new(() =>
        {
            string hash = User.GetHashedString(Password);
            Context model = new();

            User user = model.Users.SingleOrDefault(x =>
                x.Login.ToLower() == Login.ToLower()
                && x.PasswordHash.ToLower() == hash.ToLower());
            if (user == null)
            {
                MessageBox.Show("Неверный пароль");
                return;
            }
            sessionService.CurrentLoggedInUser = user;
            pageService.NavigateCommand.Execute(new DebtorsTableView());
        }, () => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password));
    }
}
