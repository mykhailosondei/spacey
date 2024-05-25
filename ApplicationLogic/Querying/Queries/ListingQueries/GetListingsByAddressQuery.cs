using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Structs;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingsByAddressQuery(Address Address) : IRequest<IEnumerable<ListingDTO>>;