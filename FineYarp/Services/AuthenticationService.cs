using StackExchange.Redis;

namespace FineYarp;

public class AuthenticationService
{
    private readonly IDatabase _database;
    
    public AuthenticationService(IConfiguration configuration)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("redis") ?? "localhost:6379");
        _database = connectionMultiplexer.GetDatabase();
    }
    
    public AppRoles Authenticate(string userName)
    {
        var role = _database.StringGet(userName);
        if (!role.HasValue)
        {
            _database.StringAppend(userName, AppRoles.Standard.ToString());
            return AppRoles.Standard;
        }
        Enum.TryParse(role.ToString(), out AppRoles mappedRole);
        return mappedRole;
    }
}