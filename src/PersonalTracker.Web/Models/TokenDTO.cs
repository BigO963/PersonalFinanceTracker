using System.Text.Json.Serialization;

namespace PersonalTracker.Web.Models;

public class TokenDTO
{
    [JsonPropertyName("accessToken")]
    public required string AccessToken { get; set; }
    [JsonPropertyName("expiresIn")]
    public required int ExpiresIn { get; set; }
    [JsonPropertyName("refreshToken")]
    public required string RefreshToken { get; set; }
    [JsonPropertyName("tokenType")]
    public required string TokenType { get; set; }
}