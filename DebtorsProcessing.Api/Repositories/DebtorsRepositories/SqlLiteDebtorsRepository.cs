using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.DebtorsRepositories
{
    public class SqlLiteDebtorsRepository : BaseSqLiteRepository<Debtor>,IDebtorsRepository
    {
        protected override Expression<Func<DebtorsContext, DbSet<Debtor>>> DbSetSelector()
        {
            return context => context.Debtors;
        }

    }
}
