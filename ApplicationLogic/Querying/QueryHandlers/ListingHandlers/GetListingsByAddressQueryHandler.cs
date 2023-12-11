using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Structs;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Querying.Queries.ListingQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

public class GetListingsByAddressQueryHandler : BaseHandler, IRequestHandler<GetListingsByAddressQuery, IEnumerable<ListingDTO>>
{
    private readonly IListingQueryRepository _listingQueryRepository;
    
    public GetListingsByAddressQueryHandler(IMapper mapper, IListingQueryRepository listingQueryRepository) : base(mapper)
    {
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task<IEnumerable<ListingDTO>> Handle(GetListingsByAddressQuery request, CancellationToken cancellationToken)
    {
        SearchType searchType = GetSearchType(request.Address);
        var result = searchType switch {
            SearchType.City => await _listingQueryRepository.GetListingsByCity(request.Address),
            SearchType.Country => await _listingQueryRepository.GetListingsByCountry(request.Address),
            SearchType.Street => await _listingQueryRepository.GetListingsByStreet(request.Address),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        return _mapper.Map<IEnumerable<ListingDTO>>(result);
    }

    private static SearchType GetSearchType(Address address)
    {
        if (address.City == null && address.Street == null)
        {
            return SearchType.Country;
        }
        return address.Street == null ? SearchType.City : SearchType.Street;
    }

    enum SearchType
    {
        City,
        Country,
        Street
    }
}