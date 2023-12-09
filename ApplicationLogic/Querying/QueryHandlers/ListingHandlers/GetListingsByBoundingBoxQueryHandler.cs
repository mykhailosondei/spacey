using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Querying.Queries.ListingQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

public class GetListingsByBoundingBoxQueryHandler : BaseHandler, IRequestHandler<GetListingsByBoundingBoxQuery, IEnumerable<ListingDTO>>
{
    private readonly IListingQueryRepository _listingQueryRepository;
    
    public GetListingsByBoundingBoxQueryHandler(IMapper mapper, IListingQueryRepository listingQueryRepository) : base(mapper)
    {
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task<IEnumerable<ListingDTO>> Handle(GetListingsByBoundingBoxQuery request, CancellationToken cancellationToken)
    {
        var result = await _listingQueryRepository.GetListingsByBoundingBox(request.BoundingBox);
        return _mapper.Map<IEnumerable<ListingDTO>>(result);
    }
}