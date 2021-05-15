using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsDbModel.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        private const string Salt = "X#c=ZkgR/aS4_HZ(HfEH(6.nqyd=Q-qkxbn!ffR=V=xiD8aq#4";

        private static string GetSaltedString([NotNull] string source)
        {
            return $"{source}{Salt}";
        }

        public static string GetHashedString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            value = GetSaltedString(value);
            StringBuilder Sb = new();

            using SHA256 hash = SHA256.Create();
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(value));
            foreach (byte b in result)
                Sb.Append(b.ToString("x2").ToLower());
            return Sb.ToString();
        }

        public ICollection<Debtor> Debtors { get; set; } = new List<Debtor>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}
