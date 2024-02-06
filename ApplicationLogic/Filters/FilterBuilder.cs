using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using MongoDB.Driver;

namespace ApplicationLogic.Filters;

public static class FilterBuilder
{
    public static FilterDefinition<Listing> BuildFilterDefinition(IEnumerable<AbstractFilter> abstractFilters)
    {
        return abstractFilters.Aggregate(Builders<Listing>.Filter.Empty, (current, filter) => current & filter.BuildDefinition());
    }
}