using ApplicationLogic.Filters;
using ApplicationLogic.Filters.Abstract;
using ApplicationLogic.Querying.QueryHandlers.ListingHandlers;
using ApplicationLogic.Visitors.Abstract;

namespace ApplicationLogic.Visitors;

public class FilterVisitorAND : IFilterVisitor
{
    private List<ListingAndBookings> _listings;

    public FilterVisitorAND(List<ListingAndBookings> listings)
    {
        _listings = listings;
    }

    public void Visit(DateFilter filter)
    {
        _listings = filter.ApplyFilter(_listings).Result;
    }

    public void Visit(GuestsFilter filter)
    {
        _listings = filter.ApplyFilter(_listings).Result;
    }

    public void Visit(PlaceFilter filter)
    {
        _listings = filter.ApplyFilter(_listings).Result;
    }

    public void Visit(PropertyTypeFilter filter)
    {
        _listings = filter.ApplyFilter(_listings).Result;
    }
}