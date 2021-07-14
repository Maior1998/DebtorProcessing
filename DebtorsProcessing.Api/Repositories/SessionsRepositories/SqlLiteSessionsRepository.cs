using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.SessionsRepositories
{
    public class SqlLiteSessionsRepository : ISessionsRepository
    {
        public async Task<UserSession> FindSessionById(Guid id)
        {
            DebtorsContext context = new();
            return await context.Sessions.SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<UserSession>> GetSessionsOfUser(Guid userId)
        {
            DebtorsContext context = new();
            return await context.Sessions.Where(x => x.User.Id == userId).ToArrayAsync();
        }

        public async Task<IEnumerable<UserSession>> GetActiveSessionsOfUser(Guid userId)
        {
            DebtorsContext context = new();
            return await context.Sessions.Where(x => x.User.Id == userId && x.EndDate == null).ToArrayAsync();
        }

        public async Task<IQueryable<UserSession>> GetAllEntities()
        {
            return new DebtorsContext().Sessions;
        }

        public async Task AddEntity(UserSession entity)
        {
            DebtorsContext context = new();
            await context.Sessions.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteEntity(Guid id)
        {
            DebtorsContext context = new();
            UserSession session = await context.Sessions.SingleAsync(x => x.Id == id);
            context.Sessions.Remove(session);
            await context.SaveChangesAsync();
        }

        public Task<UserSession> GetEntityById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEntity(UserSession entity)
        {
            throw new NotImplementedException();
        }
    }
}
