using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.EntitySecurityManagers.DebtorsSecurityManagers
{
    public class RoleBasedDebtorSecurityManager : IDebtorsSecurityManager
    {
        public Expression<Func<Debtor, bool>> CollectionSecurityFilter => deb => true;
        public bool CanUserCreateEntity(Debtor creatingEntity)
        {
            return true;
        }

        public bool CanUserModifyEntity(Debtor updatingEntity)
        {
            return true;
        }

        public bool CanUserDeleteEntity(Debtor deletingEntity)
        {
            return true;
        }
    }
}
