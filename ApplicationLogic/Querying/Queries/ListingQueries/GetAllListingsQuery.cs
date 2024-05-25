using ApplicationCommon.DTOs.Listing;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetAllListingsQuery(uint From, uint To) : IRequest<IEnumerable<ListingDTO>>;