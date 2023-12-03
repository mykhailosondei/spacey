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
            return BsonSerializer.Deserialize<HostDTO>(cachedHost);
        }
        
        var result = await _hostQueryRepository.GetHostById(request.Id);
        
        if (result == null)
        {
            throw new NotFoundException(nameof(HostDTO));
        }
        
        await _distributedCache.SetStringAsync(cacheKey, result.ToBsonDocument().ToString());
        
        return _mapper.Map<HostDTO>(result);
    }
}