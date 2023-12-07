using ApplicationCommon.DTOs.User;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.UserQueries;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace ApplicationLogic.Querying.QueryHandlers.UserHandlers;

public class GetUserByIdQueryHandler : BaseHandler, IRequestHandler<GetUserByIdQuery, UserDTO>
{
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IDistributedCache _distributedCache;
    
    public GetUserByIdQueryHandler(IMapper mapper, IUserQueryRepository userQueryRepository, IDistributedCache distributedCache) : base(mapper)
    {
        _userQueryRepository = userQueryRepository;
        _distributedCache = distributedCache;
    }

    public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"user-{request.Id}";
        var cachedUser = await _distributedCache.GetStringAsync(cacheKey);
        
        if (cachedUser != null)
        {
            var cachedUserDTO = BsonSerializer.Deserialize<UserDTO>(cachedUser);
            cachedUserDTO.LastAccess = DateTime.UtcNow;
            await _distributedCache.SetStringAsync($"user-{request.Id}-timestamp", JsonConvert.SerializeObject(cachedUserDTO.LastAccess));
            return cachedUserDTO;
        }
        
        var result = await _userQueryRepository.GetUserById(request.Id);
        
        if (result == null)
        {
            throw new NotFoundException(nameof(UserDTO));
        }
        
        await _distributedCache.SetStringAsync(cacheKey, result.ToBsonDocument().ToString());
        await _distributedCache.SetStringAsync($"user-{request.Id}-timestamp", JsonConvert.SerializeObject(result.LastAccess));
        
        return _mapper.Map<UserDTO>(result);
    }
}