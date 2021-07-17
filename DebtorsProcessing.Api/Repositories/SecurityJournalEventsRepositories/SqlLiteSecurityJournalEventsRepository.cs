using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.SecurityJournalEventsRepositories
{
    public class SqlLiteSecurityJournalEventsRepository : ISecurityJournalEventsRepository

    {
        public Task AddEntity(SecurityJournalEvent entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEntity(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SecurityJournalEvent> GetAllEntities()
        {
            throw new NotImplementedException();
        }

        public IQueryable<SecurityJournalEvent> GetEntity(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<SecurityJournalEvent> GetEntityById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEntity(SecurityJournalEvent entity)
        {
            throw new NotImplementedException();
        }
    }
}
