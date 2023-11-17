using ApplicationCommon.DTOs.Listing;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingByIdQuery(Guid Id) : IRequest<ListingDTO>;