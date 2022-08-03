using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EdgeDbDemo.Data;

public class EdgeDbClient
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializer;

    public EdgeDbClient(HttpClient client)
    {
        _client = client;

        _jsonSerializer = new JsonSerializerOptions();
        _jsonSerializer.Converters.Add(new JsonStringEnumConverter());
    }

    public async Task<IReadOnlyCollection<TResult>?> QueryAsync<TResult>(string queryText, IDictionary<string, object>? bindings = null, 
        CancellationToken cancellationToken = default)
    {
        var body = new EdgeDbQueryBody(queryText, bindings);

        var response = await _client.PostAsJsonAsync(string.Empty, body, cancellationToken);
        var queryResponse = await response.Content.ReadFromJsonAsync<EdgeQueryQueryResponse>(_jsonSerializer);

        if (queryResponse is null)
            throw new EdgeDbClientException("Unable to deserialise EdgeDb response");

        if (queryResponse.Data is null)
            return null;

        return DeserialiseData<TResult>(queryResponse.Data);
    }

    public async Task ExecuteAsync(string queryText, IDictionary<string, object>? bindings = null, CancellationToken cancellationToken = default)
    {
        var body = new EdgeDbQueryBody(queryText, bindings);

        var response = await _client.PostAsJsonAsync(string.Empty, body, cancellationToken);
        var result = await response.Content.ReadFromJsonAsync<EdgeQueryQueryResponse>();

        if (!response.IsSuccessStatusCode)
            throw new EdgeDbClientException($"Failed to execute HTTP query, Status [{response.StatusCode}]");

        if (result is null)
            throw new EdgeDbClientException("Unable to deserialise EdgeDb response");
    }

    private IReadOnlyCollection<TResult>? DeserialiseData<TResult>(object data)
    {
        var jsonElement = (JsonElement)data;

        if (jsonElement.ValueKind == JsonValueKind.Array)
        {
            var queryResponseArrayLength = jsonElement.GetArrayLength();
            var results = new TResult[queryResponseArrayLength];

            for (var i = 0; i < queryResponseArrayLength; i++)
            {
                var item = jsonElement[i];
                var itemData = item.Deserialize<TResult>(_jsonSerializer);

                if (itemData is not null)
                    results[i] = itemData;
            }

            return results;
        }

        return null;
    }
}
