using airbnb_net.Extensions;
using airbnb_net.Middlewares;
using ApplicationLogic.CloudStorage;
using ApplicationLogic.Notifications;
using ApplicationLogic.Options;


var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.RegisterCustomServices();

builder.Services.Configure<BingMapsConnectionOptions>(
    builder.Configuration.GetSection(key: nameof(BingMapsConnectionOptions)));

builder.Services.Configure<AzureCloudStorageOptions>(
    builder.Configuration.GetSection(key: nameof(AzureCloudStorageOptions)));

builder.Services.ConfigureJwt(config);
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition");
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting(); 
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserIdMiddleware>();
app.UseMiddleware<RoleMiddleware>();
app.UseMiddleware<ValidationExceptionHandlingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapHub<NotificationHub>("/message");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();