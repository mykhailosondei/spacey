using Microsoft.AspNetCore.SignalR;

namespace ApplicationLogic.SignalRIdProviders;

public class HostUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return "";
    }
}