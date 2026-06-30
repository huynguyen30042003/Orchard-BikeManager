using System.Text.Json.Serialization;

namespace BikeManagerV3.Auth.Models;

public class TokenResponse
{
    [JsonPropertyName("token_type")]
    public string? token_type { get; set; }

    [JsonPropertyName("access_token")]
    public string? access_token { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? refresh_token { get; set; }

    [JsonPropertyName("expires_in")]
    public int expires_in { get; set; }
}