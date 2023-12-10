using System.Text.Json;
using FineWeb.Dtos;

namespace FineWeb.Clients;

public class FineAppClient
{
    private readonly HttpClient _httpClient;

    public FineAppClient(string url)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(url);

    }
    
    public async Task<AppState?> GetStatus()
    {
        try
        {
            var response = await _httpClient.GetAsync("/status");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var contentStream =
                await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<AppState>(contentStream,  new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    public async Task SetUnhealthy()
    {
        await _httpClient.PostAsJsonAsync("/set-state", new AppState(false, false, string.Empty, 0));
    }
    
    public async Task EnableDelay()
    {
        await _httpClient.PostAsJsonAsync("/set-state", new AppState(true, true, string.Empty, 0));
    }
    
    public async Task ResetState()
    {
        await _httpClient.GetAsync("/reset-state");
    }
}