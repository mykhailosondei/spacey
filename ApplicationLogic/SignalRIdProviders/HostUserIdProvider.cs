using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ApplicationLogic.SignalRIdProviders;

public class HostUserIdProvider : IUserIdProvider
{
    private readonly ILogger<HostUserIdProvider> _logger;
    
    public HostUserIdProvider(ILogger<HostUserIdProvider> logger)
    {
        _logger = logger;
    }
    
    public string? GetUserId(HubConnectionContext connection)
    {
        var user = connection.User;
        var userId = user.FindFirst("id");
        var hostId = user.FindFirst("hostId");
        var isUserRole = user.IsInRole("User");
        Console.WriteLine(new string('\n', 10) + $"UserId: {userId?.Value}, HostId: {hostId?.Value}");
        _logger.LogWarning(isUserRole ? $"UserId: {userId?.Value}" : $"HostId: {hostId?.Value}");
        return isUserRole ? userId?.Value : hostId?.Value;
    }
}