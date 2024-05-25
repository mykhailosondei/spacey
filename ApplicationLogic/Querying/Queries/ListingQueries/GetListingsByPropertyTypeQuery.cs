using ApplicationCommon.DTOs.Listing;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingsByPropertyTypeQuery(string PropertyType) : IRequest<IEnumerable<ListingDTO>>;