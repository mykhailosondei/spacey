using ApplicationLogic.Services;

namespace airbnb_net.Extensions;

public static class ServiceExtensions
{
    public static void RegisterCustomServices(this IServiceCollection services)
    {
        services.AddScoped<BookingService>();
        services.AddScoped<GuestService>();
        services.AddScoped<ReviewService>();
        services.AddScoped<HostService>();
        services.AddScoped<ListingService>();
    }
}