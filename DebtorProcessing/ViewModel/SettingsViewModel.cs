using System.Linq;
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
        private DelegateCommand changePassword;

        public SettingsViewModel(SessionService session)
        {
            Session = session;
        }

        public SessionService Session { get; }

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