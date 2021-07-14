using DebtorsProcessing.Api.Repositories.RolesRepositories;
using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Controllers.OdataControllers
{
    public class UserRolesController : HelperODataController<UserRole>
    {
        public UserRolesController(IRolesRepository repository) : base(repository) { }
    }
}
