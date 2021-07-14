using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.RolesRepositories
{
    public interface IRolesRepository : IOdataEntityRepository<UserRole>
    {
        public Task<IEnumerable<UserRole>> GetRolesInSession(Guid sessionId);
        public Task<IEnumerable<UserRole>> GetRolesOfUser(Guid userId);
    }
}
