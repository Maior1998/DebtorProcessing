using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.EntitySecurityManagers.UserRolesSecurityManagers
{
    public class RoleBasedUserRolesSecurityManager : IUserRolesSecurityManager
    {
        public Expression<Func<UserRole, bool>> CollectionSecurityFilter => throw new NotImplementedException();
        public bool CanUserCreateEntity(UserRole creatingEntity)
        {
            return true;
        }

        public bool CanUserModifyEntity(UserRole updatingEntity)
        {
            return true;
        }

        public bool CanUserDeleteEntity(UserRole deletingEntity)
        {
            return true;
        }
    }
}
