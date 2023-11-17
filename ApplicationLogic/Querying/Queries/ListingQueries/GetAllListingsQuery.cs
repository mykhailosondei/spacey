using ApplicationCommon.DTOs.Listing;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetAllListingsQuery() : IRequest<IEnumerable<ListingDTO>>;