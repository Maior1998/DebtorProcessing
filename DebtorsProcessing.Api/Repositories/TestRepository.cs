using DebtorsDbModel.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories
{
    public class TestRepository : IDebtorsRepository
    {
        public Task AddRefreshTokenAsync(LoginRefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
