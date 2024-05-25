using ApplicationCommon.DTOs.Listing;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingByIdQuery(Guid Id) : IRequest<ListingDTO>;