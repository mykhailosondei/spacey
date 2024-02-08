using System.Globalization;
using System.Text;
using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

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

    public override PipelineDefinition<Listing, Listing> BuildDefinition()
    {
        var @string =
            $"{{ $match: {{ bookings: {{ $elemMatch: {{ $or: [ {{ CheckIn: {{ $gte: ISODate(\"{_checkOut.Date.ToShortDateString()}\") }} }}, {{ CheckOut: {{ $lte: ISODate(\"{_checkIn.Date.ToShortDateString()}\") }} }} ] }} }} }} }}";
        
        Console.WriteLine(@string);
        
        var aggregationPipeline = new BsonDocument[]
        {
            BsonDocument.Parse("{ $lookup: { from: 'bookings', localField: 'BookingsIds', foreignField: '_id', as: 'bookings' } }"),
            BsonDocument.Parse(@string),
            BsonDocument.Parse("{ $project: { bookings: 0 } }")
        };

        
        var pipeline = PipelineDefinition<Listing, Listing>.Create(aggregationPipeline);
        
        return pipeline;
    }
}