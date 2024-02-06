using ApplicationCommon.DTOs.Listing;
using ApplicationLogic.Abstract;
using ApplicationLogic.Filters.Abstract;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingsBySearchQuery(List<AbstractFilter> Filters) : IRequest<IEnumerable<ListingDTO>>
{
    public GetListingsBySearchQuery() : this(new List<AbstractFilter>())
    {
        
    }
}