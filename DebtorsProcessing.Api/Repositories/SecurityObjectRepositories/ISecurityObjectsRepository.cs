using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.SecurityObjectRepositories
{
    public interface ISecurityObjectsRepository 
    {
        public Task<IEnumerable<SecurityObject>> GetObjectsInSession(Guid sessionId);
    }
}
