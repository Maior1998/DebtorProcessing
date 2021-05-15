﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DebtorsDbModel.Model;

namespace DebtorProcessing.Model
{
    public static class DatabaseHelperFunctions
    {
        public static Debtor GetExampleDebtor()
        {
            return new()
            {
                FullName = "Новый должник",
                ContractNumber = "123456789",
                PassportNumber = 123456,
                PassportSeries = 1234,
                StartDebt = 100000,
                RegistrationAddress = "Тестовый адрес регистрации",
                Payments = new List<DebtorPayment>()
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
                    },
                }

            };

        }
    }
}