using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Hms.MVC.Services;

/// <summary>
/// Wraps HttpClient to call the HMS API.
/// Automatically attaches the JWT token from session to every request.
/// </summary>
public class ApiService : IApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("HmsApi");
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");

        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var client = CreateClient();
        var response = await client.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode) return default;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        var client = CreateClient();
        var content = new StringContent(
            JsonSerializer.Serialize(data, JsonOptions), Encoding.UTF8, "application/json");

        var response = await client.PostAsync(endpoint, content);

        if (!response.IsSuccessStatusCode) return default;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task<HttpResponseMessage> PostRawAsync(string endpoint, object data)
    {
        var client = CreateClient();
        var content = new StringContent(
            JsonSerializer.Serialize(data, JsonOptions), Encoding.UTF8, "application/json");

        return await client.PostAsync(endpoint, content);
    }

    public async Task<T?> PutAsync<T>(string endpoint, object data)
    {
        var client = CreateClient();
        var content = new StringContent(
            JsonSerializer.Serialize(data, JsonOptions), Encoding.UTF8, "application/json");

        var response = await client.PutAsync(endpoint, content);

        if (!response.IsSuccessStatusCode) return default;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task<T?> PatchAsync<T>(string endpoint, object data)
    {
        var client = CreateClient();
        var content = new StringContent(
            JsonSerializer.Serialize(data, JsonOptions), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Patch, endpoint) { Content = content };
        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode) return default;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        var client = CreateClient();
        var response = await client.DeleteAsync(endpoint);
        return response.IsSuccessStatusCode;
    }
}
