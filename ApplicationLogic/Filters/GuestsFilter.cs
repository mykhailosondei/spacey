using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using MongoDB.Driver;

namespace ApplicationLogic.Filters;

public class GuestsFilter : AbstractFilter
{
    private readonly int _guests;

    public GuestsFilter(int guests)
    {
        _guests = guests;
    }

    public override FilterDefinition<Listing> BuildDefinition()
    {
        throw new NotImplementedException();
    }
}