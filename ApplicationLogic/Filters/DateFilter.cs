using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationLogic.Filters;

public class DateFilter : AbstractFilter
{
    private readonly DateTime _checkIn;
    private readonly DateTime _checkOut;

    public DateFilter(DateTime checkIn, DateTime checkOut)
    {
        _checkIn = checkIn;
        _checkOut = checkOut;
    }

    interface IMongoCollectionMock<TDocument> : IMongoCollection<TDocument>
    {
        new CollectionNamespace CollectionNamespace { get; }
    }

    public override PipelineDefinition<Listing, Listing> BuildDefinition()
    {

        var aggregationPipeline = new BsonDocument[]
        {
            BsonDocument.Parse("{ $lookup: { from: 'bookings', localField: 'BookingsIds', foreignField: '_id', as: 'bookings' } }"),
            BsonDocument.Parse(string.Format(
                "{ $match: { bookings: { $not: { $elemMatch: { $and: [ { CheckIn: { $lte: \"{checkIn}\" } }, { CheckOut: { $gte: \"{checkOut}\" } } ] } } } } }",
                _checkIn,
                _checkOut)),
            BsonDocument.Parse("{ $project: { bookings: 0 } }")
        };
        
        var pipeline = PipelineDefinition<Listing, Listing>.Create(aggregationPipeline);
        
        return pipeline;
    }
}