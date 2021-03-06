using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.EntitySecurityManagers.SecurityJournalEventsSecurityManager
{
    public class RoleBasedSecurityJournalEventsSecurityManager : ISecurityJournalEventsSecurityManager
    {
        public Expression<Func<SecurityJournalEvent, bool>> CollectionSecurityFilter => throw new NotImplementedException();
        public bool CanUserCreateEntity(SecurityJournalEvent creatingEntity)
        {
            return true;
        }

        public bool CanUserModifyEntity(SecurityJournalEvent updatingEntity)
        {
            return true;
        }

        public bool CanUserDeleteEntity(SecurityJournalEvent deletingEntity)
        {
            return true;
        }
    }
}
