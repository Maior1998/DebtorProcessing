using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;


namespace DebtorsProcessing.Desktop.Model
{
    public static class DatabaseHelperFunctions
    {
        public static Debtor GetExampleDebtor()
        {
            return new()
            {
                FullName = "Новый должник",
                ContractNumber = "123456789",
                StartDebt = 100000,
                Payments = new List<DebtorPayment>
                {
                    new()
                    {
                        Amount = 132,
                        Date = DateTime.Now
                    },
                    new()
                    {
                        Amount = 3531,
                        Date = DateTime.Now.AddDays(-2)
                    },
                    new()
                    {
                        Amount = 3529,
                        Date = DateTime.Now.AddMonths(-2)
                    }
                }
            };
        }
    }
}