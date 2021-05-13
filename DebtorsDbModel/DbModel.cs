using System;
using System.Collections.Generic;

using DebtorsDbModel.Model;

using Microsoft.EntityFrameworkCore;

namespace DebtorsDbModel
{
    public class DbModel : DbContext
    {
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
                .HasOne(x => x.GrantedAccessMode)
                .WithMany(x => x.RoleObjectAccesses)
                .HasForeignKey(x => x.GrantedAccessModeId)
                .IsRequired();

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
                .HasIndex(x => new { x.GrantedAccessModeId, x.ObjectId, x.UserRoleId })
                .IsUnique();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Debtor> Debtors { get; set; }
        public virtual DbSet<DebtorPayment> DebtorPayments { get; set; }
        public virtual DbSet<AccessMode> AccessModes { get; set; }

        public static void CreateTestData(DbModel model)
        {
            model.Database.EnsureDeleted();
            model.Database.EnsureCreated();
            model.Users.Add(new User()
            {
                Login = "admin",
                PasswordHash = User.GetHashedString("admin"),
                UserRoles = new List<UserRole>()
                {
                    new UserRole(){Name = "Role 1", RoleObjectAccesses = new List<RoleObjectAccess>()
                    {

                    }
                    }
                }
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

            model.SaveChanges();
        }

    }
}
