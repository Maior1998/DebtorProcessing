using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DebtorProcessing.Services;
using DebtorProcessing.View.Windows;

using DebtorsDbModel;
using DebtorsDbModel.Model;

using DevExpress.Mvvm;

using ReactiveUI;

namespace DebtorProcessing.ViewModel
{
    public class SettingsViewModel : ReactiveObject
    {
        public SessionService Session { get; private set; }
        public SettingsViewModel(SessionService session)
        {
            Session = session;
        }

        private DelegateCommand changePassword;
        public DelegateCommand ChangePassword => changePassword ??= new(() =>
        {
            ChangePasswordWindow changePasswordWindow = new();
            if (!changePasswordWindow.ShowDialog().Value) return;
            string newPass = changePasswordWindow.NewPassword;
            Context db = new();
            User user = db.Users.Single(x => x.Id == Session.CurrentLoggedInUser.Id);
            user.PasswordHash = User.GetHashedString(newPass);
            db.SaveChanges();
            MessageBox.Show("Ваш пароль успешно изменен", "Изменение пароля", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }, () => Session != null && Session.CanUserChangeTheirPassword);
    }
}
