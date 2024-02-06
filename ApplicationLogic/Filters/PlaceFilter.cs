using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using MongoDB.Driver;

namespace ApplicationLogic.Filters;

public class PlaceFilter : AbstractFilter
{
    private readonly string _place;

    public PlaceFilter(string place)
    {
        _place = place;
    }

    public override FilterDefinition<Listing> BuildDefinition()
    {
        throw new NotImplementedException();
    }
}