using System.Text;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.DbHelper;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.CloudStorage;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Jwt;
using ApplicationLogic.PipelineBehaviors;
using ApplicationLogic.RoleLogic;
using ApplicationLogic.Services;
using ApplicationLogic.SignalRIdProviders;
using ApplicationLogic.UserIdLogic;
using CustomMediator;
using CustomMediator.Pipelines;
using CustomMediator.ServiceRegisterers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;

namespace airbnb_net.Extensions;

public static class ServiceExtensions
{
    public static void RegisterCustomServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<IRegistrar, RegistrarProxy>();
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

        services.AddScoped<IConversationCommandAccess, ConversationCommandAccess>();
        services.AddScoped<IConversationQueryRepository, ConversationQueryRepository>();
        
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
        
        services.AddSingleton<IUserIdProvider, HostUserIdProvider>();
        
        services.AddAutoMapper(ApplicationLogic.AssemblyMarker.Assembly);
        
        services.AddSingleton<IMongoDbContext>(_ =>
        {
            var connectionString = "mongodb+srv://compassuser:wBzZ4kD5ejcI1FWf@democluster.4nn3xhe.mongodb.net/";
            var databaseName = "airbnb";
            return new MongoDbContext(connectionString, databaseName);
        });
        
        BsonClassMap.RegisterClassMap<Conversation>(conversation =>
            {
            conversation.AutoMap();
            conversation.GetMemberMap(c => c.IsRead).SetDefaultValue(true);
        });
        
        var mediatorDictionary = Mediator.InitializeHandlerDictionary(new[] { typeof(ApplicationLogic.AssemblyMarker) });
        var flow = new PipelineFlow();
        flow.AddPipeline(typeof(ValidationBehavior<,>));
        
        MediatorInitializer.InitializeHandlers(services, mediatorDictionary);
        MediatorInitializer.InitializePipelines(services, mediatorDictionary, flow);
        
        var provider = services.BuildServiceProvider();
        Mediator mediator = new(serviceFactory: provider.GetRequiredService, handlers: mediatorDictionary);
        
        services.AddSingleton<IMediator>(mediator);
        
        services.AddValidatorsFromAssembly(ApplicationLogic.AssemblyMarker.Assembly);
        
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