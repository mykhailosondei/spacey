using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using ApplicationLogic.Options;
using ApplicationLogic.Querying.QueryHandlers.ListingHandlers;
using BingMapsRESTToolkit;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationLogic.Filters;

public sealed class PlaceFilter : AbstractFilter
{
    private readonly string? _place;
    private readonly BingMapsConnectionOptions _options;
    
    const double NEAR_DISTANCE = 0.4;

    public PlaceFilter(string? place, IOptions<BingMapsConnectionOptions> options)
    {
        _place = place;
        _options = options.Value;
    }
    
    protected override bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(_place);
    }

    public override async Task<List<ListingAndBookings>> ApplyFilter(List<ListingAndBookings> listings)
    {
        if (IsEmpty())
        {
            return listings;
        }
        
        var request = new GeocodeRequest()
        {
            Query = _place,
            IncludeNeighborhood = true,
            IncludeIso2 = true,
            MaxResults = 1,
            BingMapsKey = _options.BingMapsKey
        };

        var response = await request.Execute();
        
        if (response.StatusCode == 200)
        {
            var result = response.ResourceSets[0].Resources[0] as Location;
            var coordinates = result.Point.Coordinates;
            var latitude = coordinates[0];
            var longitude = coordinates[1];
            Console.WriteLine(latitude);
            Console.WriteLine(longitude);
            
            return listings.Where(listing => (listing.Listing.Latitude - latitude) < NEAR_DISTANCE && (listing.Listing.Longitude - longitude) < NEAR_DISTANCE).ToList();
        }
        
        return new List<ListingAndBookings>();
    }
}