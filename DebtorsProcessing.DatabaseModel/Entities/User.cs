using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DebtorsProcessing.DatabaseModel.Entities
{
    /// <summary>
    /// Представляет собой пользователя (субъекта) системы.
    /// </summary>
    public record User
    {

        public void ResetPassword(string newPassword)
        {
            PasswordHash = GetHashedString(newPassword);
        }

        /// <summary>
        /// Выполняет проверку пароля пользователя.
        /// </summary>
        /// <param name="user">Пользователь, пароль которого необходимо проверить.</param>
        /// <param name="pass">Пароль, который необходимо проверить.</param>
        /// <returns>Результат проверки пароля.</returns>
        public bool CheckPass(string pass)
        {
            string hash = GetHashedString(Salt, pass);
            return hash == PasswordHash;
        }
        private const int SALT_LENGTH = 32;
        private static Random random = new();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        /// <summary>
        /// Номер записи в базе данных.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ФИО пользователя.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Логин пользователя. Является обязательным на стороне БД.
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Хеш пароля пользователя. Солится.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Коллекция всех сессий данного пользователя.
        /// </summary>
        public ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();

        public string Salt { get; set; } = RandomString(SALT_LENGTH);

        /// <summary>
        /// Возвращает приписывание соли к паролю пользователя
        /// </summary>
        private static string GetSaltedString(string salt, string password)
        {
            return $"{password}{salt}";
        }

        /// <summary>
        /// Возвращает приписывание имеющейся соли к паролю пользователя
        /// </summary>
        private string GetSaltedString(string password)
        {
            return GetSaltedString(Salt, password);
        }

        /// <summary>
        /// Получает хеш из пароля пользователя.
        /// </summary>
        /// <param name="value">Строка-пароль, от которой необходимо получить хеш значение..</param>
        /// <returns></returns>
        public static string GetHashedString(string salt, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            value = GetSaltedString(salt, value);
            StringBuilder Sb = new();

            using SHA256 hash = SHA256.Create();
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(value));
            foreach (byte b in result)
                Sb.Append(b.ToString("x2").ToLower());
            return Sb.ToString();
        }

        /// <summary>
        /// Получает хеш из пароля пользователя.
        /// </summary>
        /// <param name="value">Строка-пароль, от которой необходимо получить хеш значение..</param>
        /// <returns></returns>
        public string GetHashedString(string value)
        {
            return GetHashedString(Salt, value);
        }

        /// <summary>
        /// Список должников, закрепленных за данным сотрудником.
        /// </summary>
        public ICollection<Debtor> Debtors { get; set; } = new List<Debtor>();

        /// <summary>
        /// Список ролей, которые принадлежат данному пользователю.
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public ICollection<LoginRefreshToken> LoginRefreshTokens { get; set; } = new List<LoginRefreshToken>();
    }
}
