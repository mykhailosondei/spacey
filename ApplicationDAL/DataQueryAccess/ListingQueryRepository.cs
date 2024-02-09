using System.Collections;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationCommon.GeospatialUtilities;
using ApplicationCommon.Structs;
using ApplicationDAL.DataQueryAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationDAL.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using Newtonsoft.Json;

namespace ApplicationDAL.DataQueryAccess;

public class ListingQueryRepository : BaseQueryRepository, IListingQueryRepository
{
    private readonly IMongoCollection<Listing> _collection = GetCollection<Listing>("listings");
    
    public async Task<Listing> GetListingById(Guid id)
    {
        var filter = Builders<Listing>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<Listing>> GetAllListings()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task<IEnumerable<Listing>> GetListingsByHostId(Guid hostId)
    {
        var filter = Builders<Listing>.Filter.Eq("Host.Id", hostId);
        return await _collection.Find(filter).ToListAsync();
    }
    
    public async Task<IEnumerable<Listing>> GetListingsByPropertyType(string propertyType)
    {
        var filter = Builders<Listing>.Filter.Eq("PropertyType", propertyType);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Listing>> GetListingsByBoundingBox(BoundingBox requestBoundingBox)
    {
        var lowerLeft = requestBoundingBox.LowerLeft;
        var upperRight = requestBoundingBox.UpperRight;
        
        GeoJsonPolygon<GeoJson2DCoordinates> polygon = new GeoJsonPolygon<GeoJson2DCoordinates>(
            new GeoJsonPolygonCoordinates<GeoJson2DCoordinates>(
                new GeoJsonLinearRingCoordinates<GeoJson2DCoordinates>(
                    new List<GeoJson2DCoordinates>
                    {
                        new (lowerLeft.Longitude, lowerLeft.Latitude),
                        new (lowerLeft.Longitude, upperRight.Latitude),
                        new (upperRight.Longitude, upperRight.Latitude),
                        new (upperRight.Longitude, lowerLeft.Latitude),
                        new (lowerLeft.Longitude, lowerLeft.Latitude)
                    }
                )
            )
        );
        var filter = Builders<Listing>.Filter.GeoWithin("Location", polygon);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Listing>> GetListingsByCity(Address address)
    {
        var filter = Builders<Listing>.Filter.Eq("Address.City", address.City) &
                     Builders<Listing>.Filter.Eq("Address.Country", address.Country);
        return await _collection.Find(filter, new FindOptions
        {
            Collation = new Collation("en", strength: CollationStrength.Primary),
        }).ToListAsync();
    }

    public async Task<IEnumerable<Listing>> GetListingsByCountry(Address address)
    {
        var filter = Builders<Listing>.Filter.Eq("Address.Country", address.Country);
        return await _collection.Find(filter, new FindOptions
        {
            Collation = new Collation("en", strength: CollationStrength.Primary),
        }).ToListAsync();
    }
    
    public async Task<IEnumerable<Listing>> GetListingsByStreet(Address address)
    {
        var streetWords = address.Street!.Split(' ');
        var filterQuery = streetWords.Length > 1 ? streetWords[1] : streetWords[0];
        var filter = Builders<Listing>.Filter.Regex("Address.Street", new BsonRegularExpression(filterQuery)) &
                     Builders<Listing>.Filter.Eq("Address.Country", address.Country) &
                     Builders<Listing>.Filter.Eq("Address.City", address.City);

        return await _collection.Find(filter, new FindOptions
        {
            Collation = new Collation("en", strength: CollationStrength.Primary)
        }).ToListAsync();
    }

    public async Task<IEnumerable<Listing>> GetListingsByPipeline(PipelineDefinition<Listing, Listing> pipeline)
    {
        return await _collection.Aggregate(pipeline).ToListAsync();
    }
}