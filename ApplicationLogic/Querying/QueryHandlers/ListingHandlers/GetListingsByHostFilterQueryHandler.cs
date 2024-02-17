using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Utilities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Querying.Queries.ListingQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

public class GetListingsByHostFilterQueryHandler : BaseHandler, IRequestHandler<GetListingsByHostFilterQuery, IEnumerable<ListingDTO>>
{

    private readonly IHostIdGetter _hostIdGetter;
    private readonly IListingQueryRepository _listingQueryRepository; 
    
    public GetListingsByHostFilterQueryHandler(IMapper mapper, IHostIdGetter hostIdGetter, IListingQueryRepository listingQueryRepository) : base(mapper)
    {
        _hostIdGetter = hostIdGetter;
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task<IEnumerable<ListingDTO>> Handle(GetListingsByHostFilterQuery request, CancellationToken cancellationToken)
    {
        var hostId = _hostIdGetter.HostId;
        long? amenities = null;
        if (request.Amenities != null)
        {
            amenities = (long)MapperUtilities.ConstructAmenitiesFromStringArray(request.Amenities);
        }
        var listings = await _listingQueryRepository.GetListingsByHostFilter(hostId, request.Bedrooms, request.Beds, request.Guests, amenities, request.Search);
        
        return _mapper.Map<IEnumerable<ListingDTO>>(listings);
    }
}