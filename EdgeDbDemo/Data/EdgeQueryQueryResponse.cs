using System.Text.Json.Serialization;

namespace EdgeDbDemo.Data;

public class EdgeQueryQueryResponse
{
    [JsonPropertyName("data")]
    public object? Data { get; set; } = null;

    [JsonPropertyName("error")]
    public EdgeQueryQueryResponseError? Error { get; set; } = null;
}
