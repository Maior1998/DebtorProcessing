using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.EntitySecurityManagers.UsersSecurityManagers
{
    public interface IUsersSecurityManager : IEntitySecurityManager<User>
    {
    }
}
