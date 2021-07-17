using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.EntitySecurityManagers.UserSessionsSecurityManagers
{
    public class RoleBasedUserSessionsSecurityManager : IUserSessionSecurityManager
    {
        public Expression<Func<UserSession, bool>> CollectionSecurityFilter => throw new NotImplementedException();
    }
}
