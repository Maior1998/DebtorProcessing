using System;

namespace DebtorsProcessing.Api.Dtos.Responses
{
    public record UserRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
