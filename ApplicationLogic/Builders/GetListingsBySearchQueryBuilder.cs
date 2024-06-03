using ApplicationCommon.Enums;
using ApplicationLogic.Filters;
using ApplicationLogic.Filters.Abstract;
using ApplicationLogic.Options;
using ApplicationLogic.Querying.Queries.ListingQueries;
using Microsoft.Extensions.Options;

namespace ApplicationLogic.Builders;

public class GetListingsBySearchQueryBuilder : IGetListingsBySearchQueryBuilder
{
    private GetListingsBySearchQuery _query;

    public GetListingsBySearchQueryBuilder()
    {
        _query = new GetListingsBySearchQuery();
    }

    public IGetListingsBySearchQueryBuilder WithPlace(string? place, IOptions<BingMapsConnectionOptions> bingMapsConnectionOptions)
    {
        _query.Filters.Add(new PlaceFilter(place, bingMapsConnectionOptions));
        return this;
    }

    public IGetListingsBySearchQueryBuilder WithDate(DateTime? checkIn, DateTime? checkOut)
    {
        _query.Filters.Add(new DateFilter(checkIn, checkOut));
        return this;
    }

    public IGetListingsBySearchQueryBuilder WithGuests(int? guests)
    {
        _query.Filters.Add(new GuestsFilter(guests));
        return this;
    }

    public IGetListingsBySearchQueryBuilder WithPropertyType(PropertyType? propertyType)
    {
        _query.Filters.Add(new PropertyTypeFilter(propertyType));
        return this;
    }

    public IGetListingsBySearchQueryBuilder WithPagination(uint from, uint to)
    {
        _query.From = from;
        _query.To = to;
        return this;
    }

    public GetListingsBySearchQuery Build()
    {
        return new GetListingsBySearchQuery
        {
            Filters = _query.Filters,
            From = _query.From,
            To = _query.To
        };
    }
}