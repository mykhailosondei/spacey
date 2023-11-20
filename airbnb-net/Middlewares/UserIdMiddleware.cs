using ApplicationLogic.HostIdLogic;
using ApplicationLogic.UserIdLogic;

namespace airbnb_net.Middlewares;

public class UserIdMiddleware
{
    private readonly RequestDelegate _next;
    
    public UserIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, IUserIdSetter userIdSetter, IHostIdSetter hostIdSetter)
    {
        var claims = context.User.Claims;
        var userId = claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var hostId = claims.FirstOrDefault(c => c.Type == "hostId")?.Value;
        Console.WriteLine(userId);
        Console.WriteLine(hostId);
        if (userId != null)
        {
            userIdSetter.UserId = Guid.Parse(userId);
        }
        
        if (hostId != null)
        {
            hostIdSetter.HostId = Guid.Parse(hostId);
        }
        
        await _next(context);
    }
}