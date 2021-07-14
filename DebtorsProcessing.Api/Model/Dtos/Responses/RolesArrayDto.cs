using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Model.Dtos.Responses
{
    public class RolesArrayDto
    {
        public IEnumerable<UserRoleDto> Roles { get; set; }
    }
}
