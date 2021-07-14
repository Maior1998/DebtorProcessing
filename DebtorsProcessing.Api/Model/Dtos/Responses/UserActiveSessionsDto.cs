using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Model.Dtos.Responses
{
    public record UserActiveSessionsDto
    {
        public IEnumerable<ChooseUserSessionDto> Sessions { get; set; }
    }
}
