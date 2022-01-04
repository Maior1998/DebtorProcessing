
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DebtorsProcessing.DatabaseModel.Abstractions;

namespace DebtorsProcessing.Api.EntitySecurityManagers
{
    public interface IEntitySecurityManager<T> where T : BaseEntity
    {

        public Expression<Func<T, bool>> CollectionSecurityFilter { get; }
        public bool CanUserCreateEntity(T creatingEntity);
        public bool CanUserModifyEntity(T updatingEntity);
        public bool CanUserDeleteEntity(T deletingEntity);
    }
}
