using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.SecurityObjectRepositories
{
    public class SqlLiteSecurityObjectsRepository : BaseSqLiteRepository<SecurityObject>,ISecurityObjectsRepository
    {

        public async Task<IEnumerable<SecurityObject>> GetObjectsInSession(Guid sessionId)
        {
            await using DebtorsContext context = new();
            UserSession session = await context.Sessions
                .Include(x => x.Roles)
                .ThenInclude(x => x.Objects)
                .SingleOrDefaultAsync(x => x.Id == sessionId);
            if (session == null) return Enumerable.Empty<SecurityObject>();
            IEnumerable<SecurityObject> objects = session.Roles.Aggregate(
                Enumerable.Empty<SecurityObject>(), (old, role) => role.Objects.Union(old));
            return objects;
        }

        protected override Expression<Func<DebtorsContext, DbSet<SecurityObject>>> DbSetSelector()
        {
            return context => context.SecurityObjects;
        }
    }
}
