using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ApplicationLogic.Notifications;

[Authorize(Roles = "Host, User")]
public class NotificationHub : Hub<INotificationClient>
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public async Task SendNotification(string message)
    {
        await Clients.Client(Context.ConnectionId).ReceiveNotification(message);
    }
    
    
}

public interface INotificationClient
{
    Task ReceiveNotification(string message);
}