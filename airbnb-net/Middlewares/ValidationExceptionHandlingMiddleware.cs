using System.Net;
using FluentValidation;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using JsonWriter = Newtonsoft.Json.JsonWriter;

namespace airbnb_net.Middlewares;

public class ValidationExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    
    public ValidationExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException validationException)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(validationException.Errors);
            await context.Response.WriteAsync(json);
        }
    }
}