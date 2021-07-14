using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Model.Dtos.Responses
{
    public record AuthResult
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public bool Success { get; set; }
        public string RefreshToken { get; set; }
        public List<string> Errors { get; set; }
    }
}
