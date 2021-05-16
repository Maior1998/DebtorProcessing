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
                .Entity<Debtor>()
                .HasMany(x => x.Payments)
                .WithOne(x => x.Debtor);

            modelBuilder.Entity<RoleObjectAccess>()
                .HasOne(x => x.Object)
                .WithMany(x => x.RoleObjectAccesses)
                .HasForeignKey(x => x.ObjectId)
                .IsRequired();

            modelBuilder.Entity<RoleObjectAccess>()
                .HasOne(x => x.UserRole)
                .WithMany(x => x.RoleObjectAccesses)
                .HasForeignKey(x => x.UserRoleId)
                .IsRequired();

            modelBuilder.Entity<RoleObjectAccess>()
                .HasIndex(x => new { x.ObjectId, x.UserRoleId })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(x => x.Debtors)
                .WithOne(x => x.Responsible);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Debtor> Debtors { get; set; }
        public virtual DbSet<DebtorPayment> DebtorPayments { get; set; }
        public virtual DbSet<RoleObjectAccess> RoleObjectAccesses { get; set; }
        public virtual DbSet<SecurityObject> SecurityObjects { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        public static void CreateTestData(Context model)
        {
            model.Database.EnsureDeleted();
            model.Database.EnsureCreated();
            model.Users.Add(new User()
            {
                Login = "admin",
                PasswordHash = User.GetHashedString("admin"),
                FullName = "Администратор",
                UserRoles = new List<UserRole>()
                {
                    new UserRole(){Name = "Role 1", RoleObjectAccesses = new List<RoleObjectAccess>()
                    {

                    }
                    }
                }
            });
            model.Users.Add(new User()
            {
                Login = "user",
                PasswordHash = User.GetHashedString("user"),
                FullName = "Пользователь",
            });
            model.SaveChanges();

            model.Debtors.Add(new Debtor()
            {
                FullName = "Ivanov Ivan Ivanovich",
                ContractNumber = "8357325235273",
                PassportSeries = 1234,
                PassportNumber = 123456,
                RegistrationAddress = "Bla bla bla",
                StartDebt = 1234,
                Payments = new List<DebtorPayment>()
                {
                    new DebtorPayment() {Amount = 123, Date = DateTime.Today}
                },
            });

            model.UserRoles.Add(new UserRole()
            {
                Name = "Role 2",
            });
            model.UserRoles.Add(new UserRole()
            {
                Name = "Role 3",

            });
            model.UserRoles.Add(new UserRole()
            {
                Name = "Role 4"
            });
            model.UserRoles.Add(new UserRole()
            {
                Name = "Role 5",

            });

            model.SecurityObjects.AddRange(SecurityObject.ObjectNameToIdTranslator.Select(x => new SecurityObject() { Id = x.Value, Name = x.Key }));
            model.SaveChanges();
            UserRole role = model.UserRoles
                .Include(x=>x.RoleObjectAccesses)
                .OrderBy(x=>x.Name).Last();
            User user = model.Users.Single(x => x.Login == "admin");
            role.RoleObjectAccesses.Clear();
            model.SaveChanges();
            SecurityObject[] objects = model.SecurityObjects.ToArray();
            for (int i = 0; i < objects.Length; i++)
            {
                role.RoleObjectAccesses.Add(new()
                {
                    Object = objects[i]
                });
            }

            model.SaveChanges();
            user.UserRoles.Add(role);
            model.SaveChanges();
            Console.WriteLine();


        }

    }
}
