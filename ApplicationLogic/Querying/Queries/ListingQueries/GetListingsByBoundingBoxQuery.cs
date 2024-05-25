using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.GeospatialUtilities;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingsByBoundingBoxQuery(BoundingBox BoundingBox) : IRequest<IEnumerable<ListingDTO>>;