using ApplicationCommon.DTOs.Listing;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetAllListingsQuery(uint From, uint To) : IRequest<IEnumerable<ListingDTO>>;