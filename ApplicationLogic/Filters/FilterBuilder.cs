using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationLogic.Filters;

public static class FilterBuilder
{
    public static PipelineDefinition<Listing, BsonDocument> BuildFilterDefinition(IEnumerable<AbstractFilter> abstractFilters)
    {
        //return abstractFilters.Aggregate(PipelineDefinition<Listing, BsonDocument>.Create(null), (current, filter) =>);
        throw new NotImplementedException();
    }
}