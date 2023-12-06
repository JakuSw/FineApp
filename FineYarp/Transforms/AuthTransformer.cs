using Yarp.ReverseProxy.Transforms;

namespace FineYarp.Transforms;

public class AuthTransformer : RequestTransform
{
    private readonly AuthenticationService _authenticationService;

    public AuthTransformer(IConfiguration configuration)
    {
        _authenticationService = new AuthenticationService(configuration);
    }
    public override ValueTask ApplyAsync(RequestTransformContext context)
    {
        var requestHeaders = context.ProxyRequest.Headers;
        
        if (requestHeaders.All(h => h.Key != "X-Role") &&
            requestHeaders.Any(h => h.Key == "X-Name" && h.Value.ToString() != string.Empty))
        {
            var userName = requestHeaders.FirstOrDefault(h => h.Key == "X-Name");
            var role = _authenticationService.Authenticate(userName.Value.ToString());
            context.ProxyRequest.Headers.Add("X-Role", role.ToString());
        }
        return ValueTask.CompletedTask;
    }
}