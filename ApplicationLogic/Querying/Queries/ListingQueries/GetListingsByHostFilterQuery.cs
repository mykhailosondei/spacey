using ApplicationCommon.DTOs.Listing;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingsByHostFilterQuery(int? Bedrooms, int? Beds, int? Guests, string[]? Amenities, string? Search) : IRequest<IEnumerable<ListingDTO>>;