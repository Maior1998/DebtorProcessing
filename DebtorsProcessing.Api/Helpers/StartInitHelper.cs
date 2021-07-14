using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Helpers
{
    public static class StartInitHelper
    {
        public static void Reset()
        {

            DebtorsContext context = new();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //if (context.Users.Any()) return;
            User addingUser = new()
            {
                Id = Guid.NewGuid(),
                FullName = "ASDASD",
                Login = "admin",
                UserRoles = new UserRole[]
                {
                    new()
                    {
                        Name="Admin"
                    }
                }
            };
            addingUser.ResetPassword("admin");
            context.Users.Add(addingUser);
            context.SaveChanges();
            context.Debtors.Add(new()
            {
                ContractNumber = "123456789",
                StartDebt = 100
            });
            context.Debtors.Add(new()
            {
                ContractNumber = "987654321",
                StartDebt = 200
            });

            context.SaveChanges();

        }
    }
}
