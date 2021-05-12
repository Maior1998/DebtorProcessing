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

            optionsBuilder.UseInMemoryDatabase("Test");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Debtor> Debtors { get; set; }
        public virtual DbSet<DebtorPayment> DebtorPayments { get; set; }


        public static void CreateTestData(DbModel model)
        {
            model.Database.EnsureDeleted();
            model.Database.EnsureCreated();
            model.Users.Add(new User() { Login = "admin", PasswordHash = User.GetHashedString("admin") });
            model.SaveChanges();

            model.Debtors.Add(new Debtor()
            {
                FullName = "Ivanov Ivan Ivanovich",
                ContractNumber="8357325235273",
                PassportSeries = 1234,
                PassportNumber = 123456,
                RegistrationAddress = "Bla bla bla",
                StartDebt = 1234,
                Payments = new List<DebtorPayment>()
                {
                    new DebtorPayment() {Amount = 123, Date = DateTime.Today}
                }
            });

            model.SaveChanges();
        }

    }
}
