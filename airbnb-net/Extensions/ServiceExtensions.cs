using System.Configuration;
using System.Reflection;
using System.Text;
using airbnb_net.Middlewares;
using Amazon.Runtime.Internal.Util;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.DbHelper;
using ApplicationDAL.Interfaces;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.BackgroundServices;
using ApplicationLogic.CloudStorage;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Jwt;
using ApplicationLogic.MappingProfiles;
using ApplicationLogic.RoleLogic;
using ApplicationLogic.Services;
using ApplicationLogic.UserIdLogic;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace airbnb_net.Extensions;

public static class ServiceExtensions
{
    public static void RegisterCustomServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<IAutocompleteService, AutocompleteService>();
        
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
        
        services.AddScoped<ICloudStorage, AzureCloudStorage>();
        
        services.AddScoped<JwtFactory>();
        services.AddScoped<ITokenGenerator>(s => s.GetService<JwtFactory>()!);
        
        services.AddScoped<UserIdStorageProvider>();
        services.AddScoped<IUserIdSetter>(s => s.GetService<UserIdStorageProvider>()!);
        services.AddScoped<IUserIdGetter>(s => s.GetService<UserIdStorageProvider>()!);
        
        services.AddScoped<HostIdStorageProvider>();
        services.AddScoped<IHostIdSetter>(s => s.GetService<HostIdStorageProvider>()!);
        services.AddScoped<IHostIdGetter>(s => s.GetService<HostIdStorageProvider>()!);
        
        services.AddScoped<RoleStorageProvider>();
        services.AddScoped<IRoleSetter>(s => s.GetService<RoleStorageProvider>()!);
        services.AddScoped<IRoleGetter>(s => s.GetService<RoleStorageProvider>()!);
        
        services.AddAutoMapper(ApplicationLogic.AssemblyMarker.Assembly);
        
        services.AddSingleton<IMongoDbContext>(_ =>
        {
            var connectionString = "mongodb+srv://compassuser:wBzZ4kD5ejcI1FWf@democluster.4nn3xhe.mongodb.net/";
            var databaseName = "airbnb";
            return new MongoDbContext(connectionString, databaseName);
        });
        
        services.AddValidatorsFromAssembly(ApplicationLogic.AssemblyMarker.Assembly);
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = new()
            {
                AbortOnConnectFail = false,
                Password = "MSmQBJVyE2LweeEFqKjYOOaJctkubqau",
                EndPoints = { "redis-16876.c267.us-east-1-4.ec2.cloud.redislabs.com:16876" }
            };
        });
        
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect("redis-16876.c267.us-east-1-4.ec2.cloud.redislabs.com:16876,password=MSmQBJVyE2LweeEFqKjYOOaJctkubqau")
            );

        services.AddHostedService<AccessBasedCacheInvalidator>();
        
        CollectionGetter.Initialize(services.BuildServiceProvider().GetService<IMongoDbContext>()!);
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