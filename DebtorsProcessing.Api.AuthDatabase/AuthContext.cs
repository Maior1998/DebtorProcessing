using Microsoft.EntityFrameworkCore;

using System;

namespace DebtorsProcessing.Api.AuthDatabase
{
    public class AuthContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("auth");
        }
    }
}
