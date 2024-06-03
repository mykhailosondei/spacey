using ApplicationCommon.Enums;
using ApplicationLogic.Options;
using ApplicationLogic.Querying.Queries.ListingQueries;
using Microsoft.Extensions.Options;

namespace ApplicationLogic.Builders;

public interface IGetListingsBySearchQueryBuilder
{
    IGetListingsBySearchQueryBuilder WithPlace(string? place, IOptions<BingMapsConnectionOptions> bingMapsConnectionOptions);
    IGetListingsBySearchQueryBuilder WithDate(DateTime? checkIn, DateTime? checkOut);
    IGetListingsBySearchQueryBuilder WithGuests(int? guests);
    IGetListingsBySearchQueryBuilder WithPropertyType(PropertyType? propertyType);
    IGetListingsBySearchQueryBuilder WithPagination(uint from, uint to);
    GetListingsBySearchQuery Build();
}