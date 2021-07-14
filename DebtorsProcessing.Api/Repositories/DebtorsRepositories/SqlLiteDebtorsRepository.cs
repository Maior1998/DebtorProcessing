using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.DebtorsRepositories
{
    public class SqlLiteDebtorsRepository : IDebtorsRepository
    {
        public async Task AddEntity(Debtor entity)
        {
            DebtorsContext context = new();
            await context.Debtors.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteEntity(Guid id)
        {
            DebtorsContext context = new();
            Debtor debtor = await context.Debtors.SingleAsync(x => x.Id == id);
            context.Debtors.Remove(debtor);
            await context.SaveChangesAsync();
        }

        public async Task<IQueryable<Debtor>> GetAllEntities()
        {
            return new DebtorsContext().Debtors;
        }

        public async Task<Debtor> GetEntityById(Guid id)
        {
            DebtorsContext context = new();
            return await context.Debtors.SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task UpdateEntity(Debtor entity)
        {
            throw new NotImplementedException();
        }
    }
}
