using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Repositories.UsersRepositories
{
    public interface IUsersRepository : IOdataEntityRepository<User>
    {
        public Task<User> FindUserByLogin(string login);


    }
}
