using ApplicationCommon.DTOs.User;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.UserQueries;
using AutoMapper;
using CustomMediator;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace ApplicationLogic.Querying.QueryHandlers.UserHandlers;

public class GetUserByIdQueryHandler : BaseHandler, IRequestHandler<GetUserByIdQuery, UserDTO>
{
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    
    public GetUserByIdQueryHandler(IMapper mapper, IUserQueryRepository userQueryRepository, IMemoryCache memoryCache, ILogger<GetUserByIdQueryHandler> logger) : base(mapper)
    {
        _userQueryRepository = userQueryRepository;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"User_{request.Id.ToString()}";
        
        _memoryCache.TryGetValue(cacheKey, out UserDTO cachedUser);
        
        if (cachedUser != null)
        {
            _logger.LogInformation("User: Cache hit");
            cachedUser.LastAccess = DateTime.UtcNow;
            return cachedUser;
        }
        
        _logger.LogInformation("User: Cache miss");
        
        var result = await _userQueryRepository.GetUserById(request.Id);

        if (result == null)
        {
            throw new NotFoundException(nameof(UserDTO));
        }

        result.LastAccess = DateTime.UtcNow;

        var mappedUser = _mapper.Map<UserDTO>(result);

        Console.WriteLine(mappedUser.ToBsonDocument(configurator: builder =>
        {
            
        }).ToJson());
        
        _memoryCache.Set(cacheKey, mappedUser, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });
        
        return mappedUser;
    }
}