using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.RefreshTokensRepositories
{
    public class SqlLiteRefreshTokensRepository : IRefreshTokensRepository
    {
        public async Task<LoginRefreshToken> AddRefreshTokenAsync(LoginRefreshToken refreshToken)
        {
            DebtorsContext context = new();
            await context.LoginRefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();
            return refreshToken;
        }
    }
}
