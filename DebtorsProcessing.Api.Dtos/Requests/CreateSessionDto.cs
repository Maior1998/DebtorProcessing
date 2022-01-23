using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Dtos.Requests
{
    public record CreateSessionDto
    {
        [Required]
        public Guid[] Roles { get; set; }
    }
}
