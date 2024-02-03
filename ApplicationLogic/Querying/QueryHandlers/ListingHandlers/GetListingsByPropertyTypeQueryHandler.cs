using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.ListingQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

public class GetListingsByPropertyTypeQueryHandler : BaseHandler, IRequestHandler<GetListingsByPropertyTypeQuery, IEnumerable<ListingDTO>>
{
    private readonly IListingQueryRepository _listingQueryRepository;
    
    public GetListingsByPropertyTypeQueryHandler(IMapper mapper, IListingQueryRepository listingQueryRepository) : base(mapper)
    {
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task<IEnumerable<ListingDTO>> Handle(GetListingsByPropertyTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await _listingQueryRepository.GetListingsByPropertyType(request.PropertyType);
        
        if (!result.Any())
        {
            return Array.Empty<ListingDTO>();
        }
        
        return _mapper.Map<IEnumerable<ListingDTO>>(result);
    }
}