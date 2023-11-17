using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Querying.Queries.ListingQueries;
using AutoMapper;
using MediatR;

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
        return _mapper.Map<ListingDTO>(result);
    }
}