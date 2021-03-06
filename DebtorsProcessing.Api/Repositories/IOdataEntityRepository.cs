using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories
{
    public interface IOdataEntityRepository<T> where T : class
    {
        public IQueryable<T> GetAllEntities();
        public IQueryable<T> GetEntity(Guid id);
        public Task<T> GetEntityById(Guid id);
        public Task UpdateEntity(T entity);
        public Task AddEntity(T entity);
        public Task DeleteEntity(Guid id);

    }
}
