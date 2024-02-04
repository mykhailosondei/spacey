using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.GeospatialUtilities;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetDistanceToListingQuery(Guid Id, Coordinates Coordinates) : IRequest<double>;