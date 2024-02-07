using ApplicationDAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationLogic.Filters.Abstract;

public abstract class AbstractFilter
{
    public abstract PipelineDefinition<Listing, Listing> BuildDefinition();
}