using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.GeospatialUtilities;
using ApplicationCommon.Structs;
using ApplicationDAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationDAL.Interfaces.QueryRepositories;

public interface IListingQueryRepository
{
    public Task<Listing> GetListingById(Guid id);

    public Task<IEnumerable<Listing>> GetAllListings();

    public Task<IEnumerable<Listing>> GetListingsByHostId(Guid hostId);
    public Task<IEnumerable<Listing>> GetListingsByPropertyType(string requestPropertyType);
    public Task<IEnumerable<Listing>> GetListingsByBoundingBox(BoundingBox requestBoundingBox);
    public Task<IEnumerable<Listing>> GetListingsByCity(Address address);
    public Task<IEnumerable<Listing>> GetListingsByCountry(Address address);
    public Task<IEnumerable<Listing>> GetListingsByStreet(Address address);
    public Task<IEnumerable<Listing>> GetListingsByPipeline(PipelineDefinition<Listing, Listing> pipeline);
    public Task<IEnumerable<Listing>> GetListingsByHostFilter(Guid hostId, int? bedrooms, int? beds, int? guests, long? amenities);
}