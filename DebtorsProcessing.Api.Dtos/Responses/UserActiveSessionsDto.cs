using System.Collections.Generic;

namespace DebtorsProcessing.Api.Dtos.Responses
{
    public record UserActiveSessionsDto
    {
        public IEnumerable<ChooseUserSessionDto> Sessions { get; set; }
    }
}
