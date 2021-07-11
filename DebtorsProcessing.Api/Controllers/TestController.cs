using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = Configuration.ConfigurationCostants.LoginAuthenticationScheme)]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetEnrolledInEvents()
        {
            return Ok(DateTime.UtcNow);
        }
    }
}
