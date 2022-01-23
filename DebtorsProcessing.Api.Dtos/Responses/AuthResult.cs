using System.Collections.Generic;

namespace DebtorsProcessing.Api.Dtos.Responses
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
