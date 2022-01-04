using DebtorsProcessing.Api.Attributes;
using DebtorsProcessing.Api.EntitySecurityManagers;
using DebtorsProcessing.Api.Repositories;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtorsProcessing.DatabaseModel.Abstractions;
using Microsoft.AspNetCore.Authentication;

namespace DebtorsProcessing.Api.Controllers
{
    [RequiresLoginAuthorize]
    public abstract class HelperODataController<T> : ODataController where T : BaseEntity
    {
        public readonly IOdataEntityRepository<T> Repository;

        private readonly IEntitySecurityManager<T> securityManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        protected HelperODataController(
            IOdataEntityRepository<T> repository,
            IEntitySecurityManager<T> securityManager,
            IHttpContextAccessor httpContextAccessor) : base()
        {
            this.Repository = repository;
            this.securityManager = securityManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        [EnableQuery]
        public IQueryable<T> Get()
        {
            return Repository.GetAllEntities().Where(securityManager.CollectionSecurityFilter);
        }

        [EnableQuery]
        public SingleResult<T> Get(Guid key)
        {
            return SingleResult.Create(Repository.GetEntity(key).Where(securityManager.CollectionSecurityFilter));
        }

        public async Task<IActionResult> Post(T entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!securityManager.CanUserCreateEntity(entity))
            {
                return Forbid();
            }
            await Repository.AddEntity(entity);
            return Created(entity);
        }

        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<T> product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            T entity = await Repository.GetEntityById(key);
            if (entity == null)
            {
                return NotFound();
            }

            if (!securityManager.CanUserModifyEntity(entity))
            {
                return Forbid();
            }
            product.Patch(entity);
            await Repository.UpdateEntity(entity);
            return Updated(entity);
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, T update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!securityManager.CanUserModifyEntity(update))
            {
                return Forbid();
            }
            await Repository.UpdateEntity(update);
            return Updated(update);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            T product = await Repository.GetEntityById(key);
            if (product == null)
            {
                return NotFound();
            }
            await Repository.DeleteEntity(key);
            return NoContent();
        }
    }
}
