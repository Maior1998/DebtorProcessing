using System;

namespace DebtorsProcessing.Api.Dtos.Responses
{
    public class ChooseUserSessionDto
    {
        public Guid Id { get; set; }
        public DateTime StartSessionTime { get; set; }
        public UserRoleDto[] RolesInSession { get; set; }
    }
}
