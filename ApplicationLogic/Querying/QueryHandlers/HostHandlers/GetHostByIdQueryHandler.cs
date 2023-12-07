using ApplicationCommon.DTOs.Host;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.HostQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace ApplicationLogic.Querying.QueryHandlers.HostHandlers;

public class GetHostByIdQueryHandler : BaseHandler, IRequestHandler<GetHostByIdQuery, HostDTO>
{
    private readonly IHostQueryRepository _hostQueryRepository;
    private readonly IDistributedCache _distributedCache;
    
    public GetHostByIdQueryHandler(IMapper mapper, IHostQueryRepository hostQueryRepository, IDistributedCache distributedCache) : base(mapper)
    {
        _hostQueryRepository = hostQueryRepository;
        _distributedCache = distributedCache;
    }

    public async Task<HostDTO> Handle(GetHostByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"host-{request.Id}";
        var cachedHost = await _distributedCache.GetStringAsync(cacheKey);
        
        if (cachedHost != null)
        {
            var cachedHostDTO = BsonSerializer.Deserialize<HostDTO>(cachedHost);
            cachedHostDTO.LastAccess = DateTime.UtcNow;
            await _distributedCache.SetStringAsync($"host-{request.Id}-timestamp", JsonConvert.SerializeObject(cachedHostDTO.LastAccess));
            return cachedHostDTO;
        }
        
        var result = await _hostQueryRepository.GetHostById(request.Id);
        
        if (result == null)
        {
            throw new NotFoundException(nameof(HostDTO));
        }
        
        await _distributedCache.SetStringAsync(cacheKey, result.ToBsonDocument().ToString());
        await _distributedCache.SetStringAsync($"host-{request.Id}-timestamp", JsonConvert.SerializeObject(result.LastAccess));
        
        return _mapper.Map<HostDTO>(result);
    }
}