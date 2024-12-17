using System.Text;
using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.DTOs.Review;
using ApplicationCommon.DTOs.User;
using ApplicationCommon.Structs;
using ApplicationCommon.Utilities;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.DbHelper;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Builders;
using ApplicationLogic.CloudStorage;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Jwt;
using ApplicationLogic.MappingProfiles;
using ApplicationLogic.PipelineBehaviors;
using ApplicationLogic.Querying.QueryHandlers.ListingHandlers;
using ApplicationLogic.RoleLogic;
using ApplicationLogic.Services;
using ApplicationLogic.SignalRIdProviders;
using ApplicationLogic.UserIdLogic;
using CustomMapper;
using CustomMediator;
using CustomMediator.Pipelines;
using CustomMediator.ServiceRegisterers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.GeoJsonObjectModel;
using Host = ApplicationDAL.Entities.Host;

namespace airbnb_net.Extensions;

public static class ServiceExtensions
{
    
    public static IServiceCollection AddCustomMapper(this IServiceCollection services)
    {
        var config = MapConfig.Instance;

        #region UserInit
        config.CreateMap<User, UserDTO>().ReverseMap();
        config.CreateMap<RegisterUserDTO, UserDTO>();
        config.CreateMap<UserUpdateDTO, UserDTO>().ForMember(dest => dest.Address, (src, context) => UserProfile.AddressFromString(src.Address, context.Items["BingMapsKey"].ToString()!).Result).Register();
        #endregion

        #region ReviewInit
        config.CreateMap<ReviewCreateDTO, ReviewDTO>();
        config.CreateMap<ReviewUpdateDTO, ReviewDTO>();
        config.CreateMap<ReviewDTO, Review>().ForMember(dest => dest.Ratings, (src, context) => src.Ratings.getRatingsArray()).Register();
        config.CreateMap<Review, ReviewDTO>().ForMember(dest => dest.Ratings, (src, context) => new Ratings(src.Ratings)).Register();
        #endregion

        #region ListingInit
        config.CreateMap<ListingCreateDTO, ListingDTO>()
            .ForMember(dest => dest.Host,
                (src,context) => new HostDTO
                {
                    Id = src.HostId,
                    ListingsIds = new List<Guid>()
                })
            .ForMember(dest => dest.Address, 
                (src, context) => UserProfile.AddressFromString(src.Address, context.Items["BingMapsKey"].ToString()!).Result).Register();
        config.CreateMap<ListingUpdateDTO, ListingDTO>()
            .ForMember(dest => dest.Host,
                (src, context) => new HostDTO { Id = src.HostId })
            .ForMember(dest => dest.Address,
                (src, context) => UserProfile.AddressFromString(src.Address, context.Items["BingMapsKey"].ToString()!).Result).Register();
        config.CreateMap<ListingDTO, Listing>()
            .ForMember(dest => dest.Amenities,
                (src, context) => MapperUtilities.ConstructAmenitiesFromStringArray(src.Amenities))
            .ForMember(dest => dest.Location,
                (src, context) =>
                    new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(src.Longitude, src.Latitude))).Register();
        config.CreateMap<Listing, ListingDTO>()
            .ForMember(dest => dest.Amenities,
                (src, context) => MapperUtilities.ConstructStringArrayFromAmenities(src.Amenities))
            .ForMember(dest => dest.Latitude, (src, context) => src.Location.Coordinates.Y)
            .ForMember(dest => dest.Longitude, (src, context) => src.Location.Coordinates.X).Register();
        #endregion

        #region ImageInit
        config.CreateMap<Image, ImageDTO>().ReverseMap();
        #endregion

        #region HostInit
        config.CreateMap<HostDTO, Host>().ReverseMap();
        config.CreateMap<HostCreateDTO, HostDTO>();
        config.CreateMap<HostUpdateDTO, HostDTO>();
        #endregion

        #region BookingInit
        config.CreateMap<BookingCreateDTO, BookingDTO>();
        config.CreateMap<BookingUpdateDTO, BookingDTO>();
        config.CreateMap<BookingDTO, Booking>();
        #endregion

        #region UserExpressions
        config.BuildMapExpression<User, UserDTO>();
        config.BuildMapExpression<UserDTO, User>();
        config.BuildMapExpression<RegisterUserDTO, UserDTO>();
        config.BuildMapExpression<UserUpdateDTO, UserDTO>();
        #endregion
        
        #region ReviewExpressions
        config.BuildMapExpression<ReviewCreateDTO, ReviewDTO>();
        config.BuildMapExpression<ReviewUpdateDTO, ReviewDTO>();
        config.BuildMapExpression<ReviewDTO, Review>();
        config.BuildMapExpression<Review, ReviewDTO>();
        #endregion
        
        #region ListingExpressions
        config.BuildMapExpression<ListingCreateDTO, ListingDTO>();
        config.BuildMapExpression<ListingUpdateDTO, ListingDTO>();
        config.BuildMapExpression<ListingDTO, Listing>();
        config.BuildMapExpression<Listing, ListingDTO>();
        #endregion
        
        #region ImageExpressions
        config.BuildMapExpression<Image, ImageDTO>();
        config.BuildMapExpression<ImageDTO, Image>();
        #endregion
        
        #region HostExpressions
        config.BuildMapExpression<HostDTO, Host>();
        config.BuildMapExpression<Host, HostDTO>();
        config.BuildMapExpression<HostCreateDTO, HostDTO>();
        config.BuildMapExpression<HostUpdateDTO, HostDTO>();
        #endregion
        
        #region BookingExpressions
        config.BuildMapExpression<BookingCreateDTO, BookingDTO>();
        config.BuildMapExpression<BookingUpdateDTO, BookingDTO>();
        config.BuildMapExpression<BookingDTO, Booking>();
        #endregion

        Mapper mapper = new(config);
        
        services.AddSingleton<IMapper>(mapper);
        
        return services;
    }
    
    public static void RegisterCustomServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        
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
        
        services.AddValidatorsFromAssembly(ApplicationLogic.AssemblyMarker.Assembly);
        
        services.AddAutoMapper(ApplicationLogic.AssemblyMarker.Assembly);

        services.AddCustomMapper();
        
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
        flow.AddPipeline(typeof(LoggingBehavior<,>));
        
        MediatorInitializer.InitializeHandlers(services, mediatorDictionary);
        MediatorInitializer.InitializePipelines(services, mediatorDictionary, flow);

        
        var provider = services.BuildServiceProvider();
        Mediator mediator = new(serviceFactory: provider.GetRequiredService, handlers: mediatorDictionary);
        
        
        
        services.AddSingleton<IMediator>(mediator);
        
        
        CollectionGetter.Initialize(services.BuildServiceProvider().GetService<IMongoDbContext>()!);
        
        services.AddLogging();
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