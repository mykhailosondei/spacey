using System.Configuration;
using System.Reflection;
using System.Text;
using Amazon.Runtime.Internal.Util;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Interfaces;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Jwt;
using ApplicationLogic.MappingProfiles;
using ApplicationLogic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace airbnb_net.Extensions;

public static class ServiceExtensions
{
    public static void RegisterCustomServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<IBookingCommandAccess, BookingCommandAccess>();
        services.AddScoped<IBookingQueryRepository, BookingQueryRepository>();
        services.AddScoped<IHostCommandAccess,HostCommandAccess>();
        services.AddScoped<IHostQueryRepository,HostQueryRepository>();
        services.AddScoped<IUserCommandAccess,UserCommandAccess>();
        services.AddScoped<IUserQueryRepository,UserQueryRepository>();
        services.AddScoped<IReviewQueryRepository,ReviewQueryRepository>();
        services.AddScoped<IReviewCommandAccess,ReviewCommandAccess>();
        services.AddScoped<IListingCommandAccess,ListingCommandAccess>();
        services.AddScoped<IListingQueryRepository,ListingQueryRepository>();
        services.AddScoped<IListingDeletor, ListingCommandAccess>();
        services.AddScoped<IBookingDeletor, BookingCommandAccess>();
        services.AddScoped<IReviewDeletor, ReviewCommandAccess>();
        services.AddScoped<JwtFactory>();
        services.AddScoped<ITokenGenerator>(s => s.GetService<JwtFactory>()!);
        services.AddAutoMapper(ApplicationLogic.AssemblyMarker.Assembly);
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration config)
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