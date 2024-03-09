using System.Net.Http.Headers;
using System.Text.Json;

namespace RestClients;

public abstract class RestClient
{
    private HttpClient HttpClient { get; }

    protected RestClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    protected async Task<T> GetAsync<T>(string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await HttpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var responseBytes = await response.Content.ReadAsByteArrayAsync();

        return JsonSerializer.Deserialize<T>(responseBytes);
    }
}
