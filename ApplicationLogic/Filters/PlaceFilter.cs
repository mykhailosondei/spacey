using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using ApplicationLogic.Querying.QueryHandlers.ListingHandlers;
using BingMapsRESTToolkit;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationLogic.Filters;

public class PlaceFilter : AbstractFilter
{
    private readonly string _place;
    
    const double NEAR_DISTANCE = 0.4;

    public PlaceFilter(string place)
    {
        _place = place;
    }

    public override async Task<List<ListingAndBookings>> ApplyFilter(List<ListingAndBookings> listings)
    {
        var request = new GeocodeRequest()
        {
            Query = _place,
            IncludeNeighborhood = true,
            IncludeIso2 = true,
            MaxResults = 1
        };

        var response = await request.Execute();
        
        if (response.StatusCode == 200)
        {
            var result = response.ResourceSets[0].Resources[0] as Location;
            var coordinates = result.Point.Coordinates;
            var latitude = coordinates[0];
            var longitude = coordinates[1];
            
            return listings.Where(listing => (listing.Listing.Latitude - latitude) < NEAR_DISTANCE && (listing.Listing.Longitude - longitude) < NEAR_DISTANCE).ToList();
        }
        
        return default;
    }
}