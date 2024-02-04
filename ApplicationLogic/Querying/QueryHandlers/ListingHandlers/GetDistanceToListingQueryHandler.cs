using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.GeospatialUtilities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.ListingQueries;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

public class GetDistanceToListingQueryHandler : IRequestHandler<GetDistanceToListingQuery, double>
{
    private readonly IListingQueryRepository _listingQueryRepository;

    public GetDistanceToListingQueryHandler(IListingQueryRepository listingQueryRepository)
    {
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task<double> Handle(GetDistanceToListingQuery request, CancellationToken cancellationToken)
    {
        var listing = await _listingQueryRepository.GetListingById(request.Id);
        
        if (listing == null)
        {
            throw new NotFoundException(nameof(ListingDTO));
        }
        
        return Coordinates.GetDistance(request.Coordinates, new Coordinates(latitude: listing.Location.Coordinates.Y, longitude: listing.Location.Coordinates.X));
    }
}