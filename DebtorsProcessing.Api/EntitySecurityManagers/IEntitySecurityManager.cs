
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.EntitySecurityManagers
{
    public interface IEntitySecurityManager<T> where T : class
    {

        public Expression<Func<T, bool>> CollectionSecurityFilter { get; }
    }
}
