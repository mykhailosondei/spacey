using System.Net;
using ApplicationLogic.Exceptions;
using Newtonsoft.Json;

namespace airbnb_net.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    
    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
            context.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(ex);
            await context.Response.WriteAsync(json);
        }
    }
}