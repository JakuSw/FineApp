namespace FineWeb.Clients;

public class FineYarpClient
{
    private readonly HttpClient _httpClient;
    
    public FineYarpClient(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("YarpUrl")!);
    }

    public void SetUser(string userName)
    {
        _httpClient.DefaultRequestHeaders.Add("X-Name", userName);
    }
    
    public async Task<string> Hello()
    {
        var result = await _httpClient.GetAsync("/hello");
        await using var contentStream =
            await result.Content.ReadAsStreamAsync();
        var reader = new StreamReader(contentStream);
        _httpClient.DefaultRequestHeaders.Remove("X-Name");
        return await reader.ReadToEndAsync();
    }
}