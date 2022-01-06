using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Model.Dtos.Responses
{
    public class ChooseUserSessionDto
    {
        public Guid Id { get; set; }
        public DateTime StartSessionTime { get; set; }
        public UserRoleDto[] RolesInSession { get; set; }
    }
}
