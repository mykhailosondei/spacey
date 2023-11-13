namespace ApplicationDAL.Interfaces;

public interface IListingDeletor
{
    public Task DeleteListing(Guid id);
}