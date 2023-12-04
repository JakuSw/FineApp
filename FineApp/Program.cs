using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

var appState = new AppState(true, false, builder.Configuration.GetValue<string>("AppName")!);

app.MapGet("/health",
        async () =>
        {
            if (appState.EnableDelay)
            {
                await Task.Delay(30000);
                Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
            
            return appState.Healthy
                ? Results.Ok("Healthy")
                : Results.StatusCode((int)HttpStatusCode.InternalServerError);
        })
    .WithName("Health endpoint")
    .WithOpenApi();

app.MapGet("/hello",
        () => Results.Ok($"Hello from {appState.AppName}!"))
    .WithName("Hello endpoint")
    .WithOpenApi();

app.MapGet("/reset-state",
        () =>
        { 
            appState = new AppState(true, false, builder.Configuration.GetValue<string>("AppName")!);
            return Results.Ok("Ok");
        })
    .WithName("Reset endpoint")
    .WithOpenApi();

app.MapPost("/set-state",
        (AppState newState) =>
        { 
            appState = newState with { AppName = builder.Configuration.GetValue<string>("AppName")! };
            return Results.Ok("Done");
        })
    .WithName("Set state endpoint")
    .WithOpenApi();

app.Run();

public record AppState(bool Healthy, bool EnableDelay, string AppName);