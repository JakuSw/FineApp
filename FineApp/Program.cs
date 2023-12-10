using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

var appState = new AppState(true, false, builder.Configuration.GetValue<string>("AppName") ?? "Unknown", 0);

app.MapGet("/health",
        async () =>
        {
            if (appState.EnableDelay)
            {
                await Task.Delay(30000);
                return Results.Ok();
            }
            
            return appState.Healthy
                ? Results.Ok("Healthy")
                : Results.StatusCode((int)HttpStatusCode.InternalServerError);
        })
    .WithName("Health endpoint")
    .WithOpenApi();

app.MapGet("/hello",
        async (HttpRequest request) =>
        {
            var role = GetRole(request);
            appState.Count++;
            if (string.IsNullOrEmpty(role))
            {
                await Task.Delay(2500);
                return Results.Ok($"Hello from {appState.AppName}! Consider our paid features!");
            }
            return Results.Ok($"Hello {role} from {appState.AppName}!");
        })
    .WithName("Hello endpoint")
    .WithOpenApi();

app.MapGet("/status",
        () => Results.Ok(appState))
    .WithName("Status endpoint")
    .WithOpenApi();

app.MapGet("/reset-state",
        () =>
        { 
            appState = new AppState(true, false, appState.AppName, appState.Count);
            return Results.Ok("Ok");
        })
    .WithName("Reset endpoint")
    .WithOpenApi();

app.MapPost("/set-state",
        (AppState newState) =>
        { 
            appState = new AppState(newState.Healthy, newState.EnableDelay,  appState.AppName, appState.Count);
            return Results.Ok("Done");
        })
    .WithName("Set state endpoint")
    .WithOpenApi();

app.Run();

string GetRole(HttpRequest httpRequest)
{
    var role = httpRequest.Headers.FirstOrDefault(h => h.Key == "X-Role");
    return role.Value.ToString() != "" ? $"{role.Value.ToString().ToLower()} user" : string.Empty;
}

public class AppState(bool healthy, bool enableDelay, string appName, int count)
{
    public string AppName { get; set; } = appName;
    public bool Healthy { get; set; } = healthy;
    public bool EnableDelay { get; set; } = enableDelay;
    public int Count { get; set; } = count;
}