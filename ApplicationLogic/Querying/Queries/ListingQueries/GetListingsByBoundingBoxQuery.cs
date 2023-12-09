using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.GeospatialUtilities;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingsByBoundingBoxQuery(BoundingBox BoundingBox) : IRequest<IEnumerable<ListingDTO>>;