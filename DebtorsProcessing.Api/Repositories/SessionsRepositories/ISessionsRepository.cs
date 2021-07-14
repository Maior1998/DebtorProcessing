using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.SessionsRepositories
{
    public interface ISessionsRepository : IOdataEntityRepository<UserSession>
    {
        public Task<IEnumerable<UserSession>> GetSessionsOfUser(Guid userId);
        public Task<IEnumerable<UserSession>> GetActiveSessionsOfUser(Guid userId);
        public Task<UserSession> FindSessionById(Guid id);
    }
}
