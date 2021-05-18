using System;
using System.Collections.Generic;
using System.Linq;

using DebtorsDbModel.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DebtorsDbModel
{
    public class Context : DbContext
    {
        public readonly ILoggerFactory MyLoggerFactory;

        public Context() : base()
        {
            MyLoggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //optionsBuilder.UseInMemoryDatabase("Test");
            optionsBuilder.UseSqlite(@"Data Source=./debtors.db");
            base.OnConfiguring(optionsBuilder);



        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<UserRole>()
                .HasMany(p => p.Users)
                .WithMany(p => p.UserRoles)
                .UsingEntity(j => j.ToTable("Users_Roles"));


            modelBuilder
                .Entity<UserRole>()
                .HasMany(p => p.Objects)
                .WithMany(p => p.Roles)
                .UsingEntity(j => j.ToTable("Roles_Objects"));

            modelBuilder
                .Entity<Debtor>()
                .HasMany(x => x.Payments)
                .WithOne(x => x.Debtor);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Debtors)
                .WithOne(x => x.Responsible);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Debtor> Debtors { get; set; }
        public virtual DbSet<DebtorPayment> DebtorPayments { get; set; }
        public virtual DbSet<SecurityObject> SecurityObjects { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        public static void CreateTestData(Context model)
        {
            model.Database.EnsureDeleted();
            model.Database.EnsureCreated();
            model.Users.Add(new()
            {
                Login = "admin",
                PasswordHash = User.GetHashedString("admin"),
                FullName = "Администратор",
            });
            model.Users.Add(new()
            {
                Login = "user",
                PasswordHash = User.GetHashedString("user"),
                FullName = "Пользователь",
            });
            model.SaveChanges();

            model.Debtors.Add(new()
            {
                FullName = "Ivanov Ivan Ivanovich",
                ContractNumber = "8357325235273",
                PassportSeries = 1234,
                PassportNumber = 123456,
                RegistrationAddress = "Bla bla bla",
                StartDebt = 1234,
                Payments = new List<DebtorPayment>()
                {
                    new() {Amount = 123, Date = DateTime.Today}
                },
            });

            string[] roles = {
                "Специалист по мониторингу судебных актов",
                "Руководитель отдела исполнительного производства",
                "Системный администратор"
            };

            model.UserRoles.AddRange(roles.Select(x => new UserRole() { Name = x }));


            model.SecurityObjects.AddRange(SecurityObject.ObjectNameToIdTranslator.Select(x => new SecurityObject() { Id = x.Value, Name = x.Key }));
            model.SaveChanges();
            UserRole role = model.UserRoles
                .OrderBy(x => x.Name).Last();
            User user = model.Users.Single(x => x.Login == "admin");
            model.SaveChanges();
            SecurityObject[] objects = model.SecurityObjects.ToArray();
            for (int i = 0; i < objects.Length; i++)
            {
                role.Objects.Add(objects[i]);
            }

            model.SaveChanges();
            user.UserRoles.Add(role);
            model.SaveChanges();
            Console.WriteLine();


        }

    }
}
