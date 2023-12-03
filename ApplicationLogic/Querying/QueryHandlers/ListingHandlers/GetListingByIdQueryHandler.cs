using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.ListingQueries;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

public class GetListingByIdQueryHandler : BaseHandler, IRequestHandler<GetListingByIdQuery, ListingDTO>
{
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IDistributedCache _distributedCache;
    
    public GetListingByIdQueryHandler(IMapper mapper, IListingQueryRepository listingQueryRepository, IDistributedCache distributedCache) : base(mapper)
    {
        _listingQueryRepository = listingQueryRepository;
        _distributedCache = distributedCache;
    }

    public async Task<ListingDTO> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"listing-{request.Id}";
        var cachedListing = await _distributedCache.GetStringAsync(cacheKey);
        
        if (cachedListing != null)
        {
            return BsonSerializer.Deserialize<ListingDTO>(cachedListing.ToBsonDocument());
        }
        
        var result = await _listingQueryRepository.GetListingById(request.Id);
        
        if (result == null)
        {
            throw new NotFoundException(nameof(ListingDTO));
        }
        
        await _distributedCache.SetStringAsync(cacheKey, result.ToBsonDocument().ToString());
        
        return _mapper.Map<ListingDTO>(result);
    }
}