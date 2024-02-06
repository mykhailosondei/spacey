using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Filters;
using ApplicationLogic.Querying.Queries.ListingQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

public class GetListingsBySearchQueryHandler :BaseHandler, IRequestHandler<GetListingsBySearchQuery, IEnumerable<ListingDTO>>
{
    private readonly IListingQueryRepository _listingQueryRepository;

    public GetListingsBySearchQueryHandler(IMapper mapper, IListingQueryRepository listingQueryRepository) : base(mapper)
    {
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task<IEnumerable<ListingDTO>> Handle(GetListingsBySearchQuery request, CancellationToken cancellationToken)
    {
        var filters = request.Filters;
        var filterDefinition = FilterBuilder.BuildFilterDefinition(filters);
        var listings = await _listingQueryRepository.GetListingsByFilter(filterDefinition);
        return _mapper.Map<IEnumerable<ListingDTO>>(listings);
    }
}