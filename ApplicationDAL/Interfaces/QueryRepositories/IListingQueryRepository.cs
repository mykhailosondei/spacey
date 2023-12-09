using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.GeospatialUtilities;
using ApplicationDAL.Entities;

namespace ApplicationDAL.Interfaces.QueryRepositories;

public interface IListingQueryRepository
{
    public Task<Listing> GetListingById(Guid id);

    public Task<IEnumerable<Listing>> GetAllListings();

    public Task<IEnumerable<Listing>> GetListingsByHostId(Guid hostId);
    public Task<IEnumerable<Listing>> GetListingsByPropertyType(string requestPropertyType);
    public Task<IEnumerable<Listing>> GetListingsByBoundingBox(BoundingBox requestBoundingBox);
}