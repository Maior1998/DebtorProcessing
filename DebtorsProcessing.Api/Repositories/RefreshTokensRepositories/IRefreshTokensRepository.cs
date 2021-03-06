using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.RefreshTokensRepositories
{
    public interface IRefreshTokensRepository
    {
        public Task<LoginRefreshToken> AddLoginRefreshTokenAsync(LoginRefreshToken refreshToken);
        public Task<SessionRefreshToken> AddSessionRefreshTokenAsync(SessionRefreshToken refreshToken);
    }
}
