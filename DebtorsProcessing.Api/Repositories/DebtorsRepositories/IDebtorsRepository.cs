using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.DebtorsRepositories
{
    public interface IDebtorsRepository : IOdataEntityRepository<Debtor>
    {
    }
}
