using DebtorsProcessing.Api.EntitySecurityManagers.UsersSecurityManagers;
using DebtorsProcessing.Api.Repositories.UsersRepositories;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Controllers.OdataControllers
{
    public class UsersController : HelperODataController<User>
    {


        public UsersController(
            IUsersRepository usersRepository,
            IUsersSecurityManager securityManager,
            IHttpContextAccessor httpContextAccessor)
            : base(usersRepository, securityManager, httpContextAccessor)
        {
        }


    }
}
