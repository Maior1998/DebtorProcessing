using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtorsDbModel.Model;

namespace DebtorProcessing.Services
{
    public class SessionService
    {
        /// <summary>
        /// Текущий залогиненный в систему пользователь.
        /// </summary>
        public User CurrentLoggedInUser { get; set; }
    }
}
