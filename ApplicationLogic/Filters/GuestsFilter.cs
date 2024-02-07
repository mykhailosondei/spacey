using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationLogic.Filters;

public class GuestsFilter : AbstractFilter
{
    private readonly int _guests;

    public GuestsFilter(int guests)
    {
        _guests = guests;
    }

    public override PipelineDefinition<Listing, Listing> BuildDefinition()
    {
        throw new NotImplementedException();
    }
}