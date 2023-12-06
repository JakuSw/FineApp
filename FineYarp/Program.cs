using FineYarp.Transforms;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<AuthTransformer>();
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(context =>
    {
        context.RequestTransforms.Add(context.Services.GetRequiredService<AuthTransformer>());
    });

var app = builder.Build();
app.MapReverseProxy();

app.UseHttpsRedirection();
app.Run();