using ApplicationDAL.DataQueryAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationDAL.DataQueryAccess;

public class ReviewQueryRepository : BaseQueryRepository
{
    private readonly IMongoCollection<Review> _collection = GetCollection<Review>("reviews");
    
    public async Task<Review> GetReviewById(Guid id)
    {
        var filter = Builders<Review>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<Review>> GetAllReviews()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task<IEnumerable<Review>> GetReviewsByUserId(Guid userId)
    {
        var filter = Builders<Review>.Filter.Eq("UserId", userId);
        return await _collection.Find(filter).ToListAsync();
    }
    
    public async Task<IEnumerable<Review>> GetReviewsByListingId(Guid listingId)
    {
        var filter = Builders<Review>.Filter.Eq("ListingId", listingId);
        return await _collection.Find(filter).ToListAsync();
    }
}