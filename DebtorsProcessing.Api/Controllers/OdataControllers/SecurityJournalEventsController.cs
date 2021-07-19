using DebtorsProcessing.Api.EntitySecurityManagers;
using DebtorsProcessing.Api.EntitySecurityManagers.SecurityJournalEventsSecurityManager;
using DebtorsProcessing.Api.Repositories;
using DebtorsProcessing.Api.Repositories.SecurityJournalEventsRepositories;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Controllers.OdataControllers
{
    public class SecurityJournalEventsController : HelperODataController<SecurityJournalEvent>
    {
        public SecurityJournalEventsController(
            ISecurityJournalEventsRepository repository,
            ISecurityJournalEventsSecurityManager securityManager,
            IHttpContextAccessor httpContextAccessor)
            : base(repository, securityManager, httpContextAccessor)
        {
        }
    }
}
