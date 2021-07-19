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

namespace DebtorsProcessing.Api.Controllers
{
    [RequiresLoginAuthorize]
    public abstract class HelperODataController<T> : ODataController where T : class
    {
        public IOdataEntityRepository<T> repository { get; }
        private IEntitySecurityManager<T> securityManager { get; }
        private IHttpContextAccessor httpContextAccessor { get; set; }
        protected HelperODataController(
            IOdataEntityRepository<T> repository,
            IEntitySecurityManager<T> securityManager,
            IHttpContextAccessor httpContextAccessor) : base()
        {
            this.repository = repository;
            this.securityManager = securityManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        [EnableQuery]
        public IQueryable<T> Get()
        {
            return repository.GetAllEntities().Where(securityManager.CollectionSecurityFilter);
        }

        [EnableQuery]
        public SingleResult<T> Get(Guid key)
        {
            return SingleResult.Create(repository.GetEntity(key).Where(securityManager.CollectionSecurityFilter));
        }

        public async Task<IActionResult> Post(T entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                await repository.AddEntity(entity);
            return Created(entity);
        }

        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<T> product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await repository.GetEntityById(key);
            if (entity == null)
            {
                return NotFound();
            }
            product.Patch(entity);
            await repository.UpdateEntity(entity);
            return Updated(entity);
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, T update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await repository.UpdateEntity(update);
            return Updated(update);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            var product = await repository.GetEntityById(key);
            if (product == null)
            {
                return NotFound();
            }
            await repository.DeleteEntity(key);
            return NoContent();
        }
    }
}
