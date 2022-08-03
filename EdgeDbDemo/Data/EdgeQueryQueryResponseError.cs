using System.Text.Json.Serialization;

namespace EdgeDbDemo.Data;

public class EdgeQueryQueryResponseError
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string ErrorType { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public int ErrorCode { get; set; } = 0;
}