using DebtorsProcessing.Api.EntitySecurityManagers.UserRolesSecurityManagers;
using DebtorsProcessing.Api.Repositories.RolesRepositories;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Controllers.OdataControllers
{
    public class UserRolesController : HelperODataController<UserRole>
    {
        public UserRolesController(
            IRolesRepository repository,
            IUserRolesSecurityManager securityManager,
            IHttpContextAccessor httpContextAccessor)
            : base(repository, securityManager, httpContextAccessor) { }
    }
}
