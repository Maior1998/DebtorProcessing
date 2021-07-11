using System.Linq;
using System.Windows;

using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;
using DebtorsProcessing.Desktop.Services;
using DebtorsProcessing.Desktop.View.Windows;

using DevExpress.Mvvm;

using ReactiveUI;

namespace DebtorsProcessing.Desktop.ViewModel
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
            DebtorsContext db = new();
            User user = db.Users.Single(x => x.Id == Session.UserId);
            //user.PasswordHash = User.GetHashedString(newPass);
            db.SaveChanges();
            MessageBox.Show("Ваш пароль успешно изменен", "Изменение пароля", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }, () => Session != null && Session.CanUserChangeTheirPassword);
    }
}