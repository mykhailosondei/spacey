using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class ReviewCommandHandler : BaseAccessHandler
{
    private readonly IMongoCollection<Review> _collection = GetCollection<Review>("reviews");
    
    public async Task AddReview(Review review)
    {
        await _collection.InsertOneAsync(review);
    }
    
    public async Task UpdateReview(Guid id, Review review)
    {
        var filter = Builders<Review>.Filter.Eq("Id", id);
        await _collection.ReplaceOneAsync(filter, review);
    }
    
    public async Task DeleteReview(Guid id)
    {
        var filter = Builders<Review>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
    }
}