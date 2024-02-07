using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using BingMapsRESTToolkit;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationLogic.Filters;

public class PlaceFilter : AbstractFilter
{
    private readonly string _place;

    public PlaceFilter(string place)
    {
        _place = place;
    }

    public override PipelineDefinition<Listing, Listing> BuildDefinition()
    {
        
        throw new NotImplementedException();
        
        var request = new GeocodeRequest()
        {
            Query = _place
        };
        
        var response = request.Execute().Result;
        
        //if (response.StatusCode == 200)
        //{
        //    var point = response.ResourceSets[0].Resources[0] as Location;
        //    if(point == null) return FilterDefinition<Listing>.Empty;
        //    return Builders<Listing>.Filter.Near(x => x.Location, point.Point.Coordinates[0], point.Point.Coordinates[1], 30000);
        //}
        //
        //return FilterDefinition<Listing>.Empty;
    }
}