using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DebtorsProcessing.DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace DebtorsProcessing.Api.Repositories.SecurityJournalEventsRepositories
{
    public class SqlLiteSecurityJournalEventsRepository : BaseSqLiteRepository<SecurityJournalEvent>,ISecurityJournalEventsRepository
    {
        protected override Expression<Func<DebtorsContext, DbSet<SecurityJournalEvent>>> DbSetSelector()
        {
            return context => context.SecurityJournalEvents;
        }

    }
}
