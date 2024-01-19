using System.Security.Claims;
using ApplicationLogic.RoleLogic;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver.Linq;

namespace airbnb_net.Middlewares;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;
    
    public RoleMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, IRoleSetter roleSetter)
    {
        var claims = context.User.Claims;
        var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        
        roleSetter.Roles = roles;
        
        await _next(context);
    }
}