using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.ListingQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

public class GetAllListingsQueryHandler : BaseHandler, IRequestHandler<GetAllListingsQuery, IEnumerable<ListingDTO>>
{
    private readonly IListingQueryRepository _listingQueryRepository;
    
    public GetAllListingsQueryHandler(IMapper mapper, IListingQueryRepository listingQueryAccess) : base(mapper)
    {
        _listingQueryRepository = listingQueryAccess;
    }

    public async Task<IEnumerable<ListingDTO>> Handle(GetAllListingsQuery request, CancellationToken cancellationToken)
    {
        var result = await _listingQueryRepository.GetAllListings();

        if (!result.Any())
        {
            return Array.Empty<ListingDTO>();
        }
        
        return _mapper.Map<IEnumerable<ListingDTO>>(result);
    }
}