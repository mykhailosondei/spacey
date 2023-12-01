using ApplicationCommon.DTOs.Listing;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingsByPropertyTypeQuery(string PropertyType) : IRequest<IEnumerable<ListingDTO>>;