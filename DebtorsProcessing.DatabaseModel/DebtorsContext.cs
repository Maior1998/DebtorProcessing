using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;

namespace DebtorsProcessing.DatabaseModel
{
    public class DebtorsContext : DbContext
    {
        static DebtorsContext()
        {
            DebtorsContext context = new();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            CreateTestData(context);
            context.Dispose();
        }

        public readonly ILoggerFactory MyLoggerFactory;

        public DebtorsContext() : base()
        {
            MyLoggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
                .UsingEntity(j => j.ToTable("UserInRole"));


            modelBuilder
                .Entity<UserRole>()
                .HasMany(p => p.Objects)
                .WithMany(p => p.Roles)
                .UsingEntity(j => j.ToTable("ObjectInRole"));

            modelBuilder
                .Entity<UserRole>()
                .HasMany(p => p.UsedInSessions)
                .WithMany(p => p.Roles)
                .UsingEntity(j => j.ToTable("RoleInSession"));

            modelBuilder
                .Entity<User>()
                .HasMany(x => x.Sessions)
                .WithOne(x => x.User);

            modelBuilder
                .Entity<Debtor>()
                .HasMany(x => x.Payments)
                .WithOne(x => x.Debtor)
                .HasForeignKey(x => x.DebtorId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Debtors)
                .WithOne(x => x.Responsible)
                .HasForeignKey(x => x.ResponsibleId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Sessions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.SecurityJournalEvents)
                .WithOne(x => x.RelatedUser)
                .HasForeignKey(x => x.RelatedUserId);

            modelBuilder.Entity<SecurityJournalEventType>()
                .HasMany(x => x.Events)
                .WithOne(x => x.Type)
                .HasForeignKey(x => x.TypeId);

            modelBuilder.Entity<User>()
                .HasOne(x => x.ActiveSession)
                .WithOne(x => x.UserByActiveSession)
                .HasForeignKey<User>(x => x.ActiveSessionId);

            modelBuilder.Entity<Debtor>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Debtor)
                .HasForeignKey(x => x.DebtorId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId);
        }
        public virtual DbSet<DebtorComment> DebtorComments { get; set; }
        public virtual DbSet<LoginRefreshToken> LoginRefreshTokens { get; set; }
        public virtual DbSet<SessionRefreshToken> SessionRefreshTokens { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Debtor> Debtors { get; set; }
        public virtual DbSet<DebtorPayment> DebtorPayments { get; set; }
        public virtual DbSet<SecurityObject> SecurityObjects { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserSession> Sessions { get; set; }
        public virtual DbSet<SecurityJournalEvent> SecurityJournalEvents { get; set; }
        public virtual DbSet<SecurityJournalEventType> SecurityJournalEventTypes { get; set; }
        public static void CreateTestData(DebtorsContext model)
        {
            if (model.Debtors.Any()) return;
            model.Database.EnsureDeleted();
            model.Database.EnsureCreated();
            User bufferUser = new()
            {
                Login = "admin",
                FullName = "Администратор",
                Sessions =
                {
                    new()
                    {
                        StartDate=DateTime.Now
                    }
                }
            };
            bufferUser.ResetPassword("admin");
            model.Users.Add(bufferUser);
            bufferUser = new()
            {
                Login = "user",
                FullName = "Пользователь",
            };
            bufferUser.ResetPassword("user");
            model.Users.Add(bufferUser);
            model.SaveChanges();

            model.Debtors.Add(new()
            {
                FullName = "Ivanov Ivan Ivanovich",
                ContractNumber = "8357325235273",
                StartDebt = 1234,
                RegistrationRegion = new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Москва"
                },
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
