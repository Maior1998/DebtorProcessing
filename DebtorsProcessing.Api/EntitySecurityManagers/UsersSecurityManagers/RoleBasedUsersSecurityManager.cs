using System;
using System.Linq.Expressions;
using DebtorsProcessing.DatabaseModel.Entities;

namespace DebtorsProcessing.Api.EntitySecurityManagers.UsersSecurityManagers
{
    public class RoleBasedUsersSecurityManager : IUsersSecurityManager
    {
        public Expression<Func<User, bool>> CollectionSecurityFilter => x => true;
        public bool CanUserCreateEntity(User creatingEntity)
        {
            throw new NotImplementedException();
        }

        public bool CanUserModifyEntity(User updatingEntity)
        {
            throw new NotImplementedException();
        }

        public bool CanUserDeleteEntity(User deletingEntity)
        {
            throw new NotImplementedException();
        }

        public bool CanUserUpdateOwnPassword(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
