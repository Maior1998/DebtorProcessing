using DebtorsProcessing.Api.Attributes;
using DebtorsProcessing.Api.EntitySecurityManagers.DebtorsSecurityManagers;
using DebtorsProcessing.Api.Repositories;
using DebtorsProcessing.Api.Repositories.DebtorsRepositories;
using DebtorsProcessing.DatabaseModel.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Controllers.OdataControllers
{

    public class DebtorsController : HelperODataController<Debtor>
    {
        public DebtorsController(
            IDebtorsRepository debtorsRepository,
            IDebtorsSecurityManager debtorsSecurityManager) 
            : base(debtorsRepository, debtorsSecurityManager) { }


    }
}
