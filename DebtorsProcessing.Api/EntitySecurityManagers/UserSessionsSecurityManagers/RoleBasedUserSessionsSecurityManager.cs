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
        public Expression<Func<UserSession, bool>> CollectionSecurityFilter => session=>true;
        public bool CanUserCreateEntity(UserSession creatingEntity)
        {
            return true;
        }

        public bool CanUserModifyEntity(UserSession updatingEntity)
        {
            return true;
        }

        public bool CanUserDeleteEntity(UserSession deletingEntity)
        {
            return true;
        }
    }
}
