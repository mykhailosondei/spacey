using ApplicationCommon.DTOs.Listing;
using ApplicationLogic.Abstract;
using ApplicationLogic.Filters.Abstract;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ListingQueries;

public record GetListingsBySearchQuery(List<AbstractFilter> Filters) : IRequest<IEnumerable<ListingDTO>>
{
    public uint From { get; set; }
    public uint To { get; set; }
    public GetListingsBySearchQuery() : this(new List<AbstractFilter>())
    {
        From = 0;
        To = int.MaxValue;
    }
}