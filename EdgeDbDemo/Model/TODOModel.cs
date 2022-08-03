using System.Text.Json.Serialization;

namespace EdgeDbDemo.Model;

public class TODOModel
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("date_created")]
    public DateTimeOffset DateCreated { get; set; }

    [JsonPropertyName("state")]
    public TODOState State { get; set; }
}
