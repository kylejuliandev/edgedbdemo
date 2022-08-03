using System.Text.Json.Serialization;

namespace EdgeDbDemo.Data;

public class EdgeDbQueryBody
{
    [JsonPropertyName("query")]
    public string QueryText { get; }

    [JsonPropertyName("variables")]
    public IDictionary<string, object> Bindings { get; }

    public EdgeDbQueryBody(string queryText, IDictionary<string, object>? bindings = null)
    {
        QueryText = queryText;
        Bindings = bindings ?? new Dictionary<string, object>();
    }
}
