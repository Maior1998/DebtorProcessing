using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.SessionsRepositories
{
    public class SqlLiteSessionsRepository : BaseSqLiteRepository<UserSession>,ISessionsRepository
    {
        public async Task<IEnumerable<UserSession>> GetSessionsOfUser(Guid userId)
        {
            await using DebtorsContext context = new();
            return await DbSetFunc(context).Where(x => x.User.Id == userId).ToArrayAsync();
        }

        public async Task<IEnumerable<UserSession>> GetActiveSessionsOfUser(Guid userId)
        {
            await using DebtorsContext context = new();
            return await DbSetFunc(context).Where(x => x.User.Id == userId && x.EndDate == null).ToArrayAsync();
        }

        protected override Expression<Func<DebtorsContext, DbSet<UserSession>>> DbSetSelector()
        {
            return context => context.Sessions;
        }
    }
}
