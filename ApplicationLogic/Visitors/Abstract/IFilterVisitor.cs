using ApplicationLogic.Filters;
using ApplicationLogic.Filters.Abstract;

namespace ApplicationLogic.Visitors.Abstract;

public interface IFilterVisitor
{
    public void Visit(DateFilter filter);
    public void Visit(GuestsFilter filter);
    public void Visit(PlaceFilter filter);
    public void Visit(PropertyTypeFilter filter);
}