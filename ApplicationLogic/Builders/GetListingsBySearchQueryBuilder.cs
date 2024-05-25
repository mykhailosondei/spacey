using ApplicationCommon.Enums;
using ApplicationLogic.Filters;
using ApplicationLogic.Filters.Abstract;
using ApplicationLogic.Options;
using ApplicationLogic.Querying.Queries.ListingQueries;
using Microsoft.Extensions.Options;

namespace ApplicationLogic.Builders;

public class GetListingsBySearchQueryBuilder
{
    private List<AbstractFilter> _filters;
    private uint _from;
    private uint _to;

    public GetListingsBySearchQueryBuilder()
    {
        _filters = new List<AbstractFilter>();
        _from = 0;
        _to = uint.MaxValue;
    }

    public GetListingsBySearchQueryBuilder WithPlace(string? place, IOptions<BingMapsConnectionOptions> bingMapsConnectionOptions)
    {
        _filters.Add(new PlaceFilter(place, bingMapsConnectionOptions));
        return this;
    }

    public GetListingsBySearchQueryBuilder WithDate(DateTime? checkIn, DateTime? checkOut)
    {
        _filters.Add(new DateFilter(checkIn, checkOut));
        return this;
    }

    public GetListingsBySearchQueryBuilder WithGuests(int? guests)
    {
        _filters.Add(new GuestsFilter(guests));
        return this;
    }

    public GetListingsBySearchQueryBuilder WithPropertyType(PropertyType? propertyType)
    {
        _filters.Add(new PropertyTypeFilter(propertyType));
        return this;
    }

    public GetListingsBySearchQueryBuilder WithPagination(uint from, uint to)
    {
        _from = from;
        _to = to;
        return this;
    }

    public GetListingsBySearchQuery Build()
    {
        return new GetListingsBySearchQuery
        {
            Filters = _filters,
            From = _from,
            To = _to
        };
    }
}