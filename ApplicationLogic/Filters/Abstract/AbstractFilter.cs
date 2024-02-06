using ApplicationDAL.Entities;
using MongoDB.Driver;

namespace ApplicationLogic.Filters.Abstract;

public abstract class AbstractFilter
{
    public abstract FilterDefinition<Listing> BuildDefinition();
}