using DebtorsProcessing.Api.Model.Dtos.Responses;
using DebtorsProcessing.DatabaseModel.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Helpers
{
    public static class DebtorsModelExtensions
    {
        public static UserRoleDto ToDto(this UserRole userRole)
        {
            return new() { Name = userRole.Name };
        }
    }
}
