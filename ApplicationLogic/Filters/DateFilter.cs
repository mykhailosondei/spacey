using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
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

    public override FilterDefinition<Listing> BuildDefinition()
    {
        throw new NotImplementedException();
    }
}