using System.Collections.Generic;

namespace DebtorsProcessing.Api.Dtos.Responses
{
    public class RolesArrayDto
    {
        public IEnumerable<UserRoleDto> Roles { get; set; }
    }
}
