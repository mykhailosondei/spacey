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
using Newtonsoft.Json;

namespace ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

public class GetListingByIdQueryHandler : BaseHandler, IRequestHandler<GetListingByIdQuery, ListingDTO>
{
    private readonly IListingQueryRepository _listingQueryRepository;
    
    public GetListingByIdQueryHandler(IMapper mapper, IListingQueryRepository listingQueryRepository) : base(mapper)
    {
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task<ListingDTO> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
    {
        
        var result = await _listingQueryRepository.GetListingById(request.Id);
        
        if (result == null)
        {
            throw new NotFoundException(nameof(ListingDTO));
        }
        
        result.LastAccess = DateTime.UtcNow;
        
        var listingDTO = _mapper.Map<ListingDTO>(result);
        
        return listingDTO;
    }
}