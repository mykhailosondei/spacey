using ApplicationCommon.Enums;
using ApplicationLogic.Filters.Abstract;
using ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

namespace ApplicationLogic.Filters;

public class PropertyTypeFilter : AbstractFilter
{
    
    private readonly PropertyType? _propertyType;
    
    public PropertyTypeFilter(PropertyType? propertyType)
    {
        _propertyType = propertyType;
    }

    public override async Task<List<ListingAndBookings>> ApplyFilter(List<ListingAndBookings> listings)
    {
        if (_propertyType == null)
        {
            return listings;
        }
        
        return listings.Where(listing => listing.Listing.PropertyType == _propertyType).ToList();
    }

    protected override bool IsEmpty()
    {
        return _propertyType == null;
    }
}