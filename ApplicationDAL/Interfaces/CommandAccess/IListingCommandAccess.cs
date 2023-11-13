using ApplicationDAL.Entities;

namespace ApplicationDAL.DataCommandAccess;

public interface IListingCommandAccess
{
    public Task<Guid> AddListing(Listing listing);

    public Task UpdateListing(Guid id, Listing listing);

    public Task DeleteListing(Guid id);
}