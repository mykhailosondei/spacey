using ApplicationLogic.UserIdLogic;

namespace airbnb_net.Middlewares;

public class UserIdMiddleware
{
    private RequestDelegate _next;
    
    public UserIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, IUserIdSetter userIdSetter)
    {
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        Console.WriteLine(userId);
        if (userId != null)
        {
            userIdSetter.UserId = Guid.Parse(userId);
        }
        
        await _next(context);
    }
}