using System.Configuration;
using System.Reflection;
using System.Text;
using Amazon.Runtime.Internal.Util;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationLogic.Jwt;
using ApplicationLogic.MappingProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace airbnb_net.Extensions;

public static class ServiceExtensions
{
    public static void RegisterCustomServices(this IServiceCollection services)
    {
        services.AddScoped<BookingCommandAccess>();
        services.AddScoped<BookingQueryRepository>();
        services.AddScoped<HostCommandAccess>();
        services.AddScoped<HostQueryRepository>();
        services.AddScoped<UserCommandAccess>();
        services.AddScoped<UserQueryRepository>();
        services.AddScoped<ReviewQueryRepository>();
        services.AddScoped<ReviewCommandAccess>();
        services.AddScoped<ListingCommandAccess>();
        services.AddScoped<ListingQueryRepository>();
        services.AddAutoMapper(ApplicationLogic.AssemblyMarker.Assembly);
    }

    public static void ConfigureJwt(this IServiceCollection services, ConfigurationManager config)
    {
        var issuerKey = config["JwtSettings:Key"];
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(issuerKey));

        var jwtSettings = config.GetSection("JwtSettings");

        services.Configure<JwtIssuerOptions>(opts =>
        {
            opts.Issuer = jwtSettings["Issuer"];
            opts.Audience = jwtSettings["Audience"];
            opts.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        });
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings[nameof(JwtIssuerOptions.Issuer)],

            ValidateAudience = true,
            ValidAudience = jwtSettings[nameof(JwtIssuerOptions.Audience)],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,

            RequireExpirationTime = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(authOpts =>
        {
            authOpts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOpts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(configureOptions =>
        {
            configureOptions.ClaimsIssuer = jwtSettings["Issuer"];
            configureOptions.TokenValidationParameters = tokenValidationParameters;
            configureOptions.SaveToken = true;
        });
    }
}