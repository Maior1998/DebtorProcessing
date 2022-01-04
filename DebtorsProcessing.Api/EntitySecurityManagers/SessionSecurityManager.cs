using DebtorsProcessing.Api.Repositories.SecurityObjectRepositories;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DebtorsProcessing.DatabaseModel.Abstractions;

namespace DebtorsProcessing.Api.EntitySecurityManagers
{
    public abstract class SessionSecurityManager<T> : IEntitySecurityManager<T> where T : BaseEntity
    {
        protected Guid sessionId, userId;
        public IEnumerable<SecurityObject> grantedObjects;

        public SessionSecurityManager(
            IHttpContextAccessor httpContextAccessor,
            ISecurityObjectsRepository repository)
        {
            UserSession session = (UserSession)httpContextAccessor.HttpContext.Items["Session"];
            sessionId = session.Id;
            userId = session.UserId.Value;
            grantedObjects = repository.GetObjectsInSession(sessionId).Result;


        }
        public Expression<Func<T, bool>> CollectionSecurityFilter => throw new NotImplementedException();

        public bool CanUserCreateEntity(T creatingEntity)
        {
            return true;
        }

        public bool CanUserModifyEntity(T updatingEntity)
        {
            return true;
        }

        public bool CanUserDeleteEntity(T deletingEntity)
        {
            throw new NotImplementedException();
        }

        public bool CanUserUpdateEntity(T updatingEntity)
        {
            throw new NotImplementedException();
        }
    }
}
