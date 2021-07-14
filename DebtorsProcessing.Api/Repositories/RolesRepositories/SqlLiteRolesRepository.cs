using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.RolesRepositories
{
    public class SqlLiteRolesRepository : IRolesRepository
    {
        public async Task<IEnumerable<UserRole>> GetRolesOfUser(Guid userId)
        {
            DebtorsContext context = new();
            User user = await context.Users.Include(x => x.UserRoles).SingleAsync(x => x.Id == userId);
            return user.UserRoles.OrderBy(x => x.Name);
        }
        public async Task<IEnumerable<UserRole>> GetRolesInSession(Guid sessionId)
        {
            DebtorsContext context = new();
            return await context.UserRoles.Where(x => x.UsedInSessions.Any(x => x.Id == sessionId)).ToArrayAsync();
        }

        public async Task<IQueryable<UserRole>> GetAllEntities()
        {
            return new DebtorsContext().UserRoles;
        }

        public async Task AddEntity(UserRole entity)
        {
            DebtorsContext context = new();
            await context.UserRoles.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteEntity(Guid id)
        {
            DebtorsContext context = new();
            UserRole role = await context.UserRoles.SingleAsync(x => x.Id == id);
            context.UserRoles.Remove(role);
            await context.SaveChangesAsync();
        }

        public Task<UserRole> GetEntityById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEntity(UserRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
