using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.UsersRepositories
{
    public class SqlLiteUsersRepository : IUsersRepository
    {
        public Task<User> FindUserByLogin(string login)
        {
            DebtorsContext context = new();
            return context.Users.SingleOrDefaultAsync(x => x.Login == login);
        }

        public Task<User> FindUserById(Guid id)
        {
            DebtorsContext context = new();
            return context.Users.SingleOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<User> GetAllEntities()
        {
            return new DebtorsContext().Users;
        }

        public async Task AddEntity(User entity)
        {
            DebtorsContext context = new();
            await context.Users.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteEntity(Guid id)
        {
            DebtorsContext context = new();
            User user = await context.Users.SingleAsync(x => x.Id == id);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }

        public Task<User> GetEntityById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEntity(User entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetEntity(Guid id)
        {
            return new DebtorsContext().Users.Where(x => x.Id == id);
        }
    }
}
