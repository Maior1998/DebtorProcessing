﻿using System;
using System.Linq;
using System.Windows;

using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;
using DebtorsProcessing.Desktop.Model;
using DebtorsProcessing.Desktop.Services;
using DebtorsProcessing.Desktop.View.Pages;

using DevExpress.Mvvm;

using Microsoft.EntityFrameworkCore;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DebtorsProcessing.Desktop.ViewModel
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
           try
           {
               bool result = await ServiceTalker.Login(Login, Password);
               if (!result)
               {
                   MessageBox.Show("Неверный логин или пароль", "Ошибка аутентификации", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                   return;
               }
               Application.Current.Dispatcher.Invoke(() =>
                   pageService.NavigateCommand.Execute(new ChooseSessionPage()));
           }
           catch (Exception ex)
           {
               MessageBox.Show($"Произошла ошибка во время аутентификации. Проверьте введенные логин и пароль.\n Полный текст ошибки:{ex.Message}", "Ошибка аутентификации", MessageBoxButton.OK,
                   MessageBoxImage.Error);
               return;
           }

       }, () => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password));
    }
}