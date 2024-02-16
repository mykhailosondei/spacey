using ApplicationCommon.DTOs.Listing;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingsByHostFilterQuery(int? Bedrooms, int? Beds, int? Guests, string[]? Amenities) : IRequest<IEnumerable<ListingDTO>>;