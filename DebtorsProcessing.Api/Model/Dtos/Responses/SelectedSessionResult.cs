using System.Collections.Generic;

namespace DebtorsProcessing.Api.Model.Dtos.Responses;

public record SelectedSessionResult
{
    public string Token { get; set; }
    public bool Success { get; set; }
    public string RefreshToken { get; set; }
    public List<string> Errors { get; set; }
}