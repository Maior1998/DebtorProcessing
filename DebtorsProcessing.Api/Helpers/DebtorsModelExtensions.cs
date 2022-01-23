using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtorsProcessing.Api.Dtos.Responses;

namespace DebtorsProcessing.Api.Helpers
{
    public static class DebtorsModelExtensions
    {
        public static UserRoleDto ToDto(this UserRole userRole)
        {
            return new() { Name = userRole.Name, Id = userRole.Id };
        }
    }
}
