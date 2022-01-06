using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.UsersRepositories
{
    public class SqlLiteUsersRepository : BaseSqLiteRepository<User>, IUsersRepository
    {
        public async Task<User> FindUserByLogin(string login)
        {
            return await GetAllEntities().SingleOrDefaultAsync(x => x.Login == login);
        }

        public async Task SetUserActiveSession(Guid userId, Guid? sessionId)
        {
            User user = await GetEntityById(userId);
            user.ActiveSessionId = sessionId;
            await UpdateEntity(user);
        }

        public async Task<Guid?> GetUserActiveSession(Guid userId)
        {
            User user = await GetEntityById(userId);
            return user.ActiveSessionId;
        }

        protected override Expression<Func<DebtorsContext, DbSet<User>>> DbSetSelector()
        {
            return context => context.Users;
        }
    }
}
