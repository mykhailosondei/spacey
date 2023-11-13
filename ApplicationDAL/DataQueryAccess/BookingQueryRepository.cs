using ApplicationDAL.DataQueryAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationDAL.DataQueryAccess;

public class BookingQueryRepository : BaseQueryRepository, IBookingQueryRepository
{
    private readonly IMongoCollection<Booking> _collection = GetCollection<Booking>("bookings");
    
    public async Task<Booking> GetBookingById(Guid id)
    {
        var filter = Builders<Booking>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<Booking>> GetAllBookings()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task<IEnumerable<Booking>> GetBookingsByUserId(Guid userId)
    {
        var filter = Builders<Booking>.Filter.Eq("UserId", userId);
        return await _collection.Find(filter).ToListAsync();
    }
    
    public async Task<IEnumerable<Booking>> GetBookingsByListingId(Guid listingId)
    {
        var filter = Builders<Booking>.Filter.Eq("ListingId", listingId);
        return await _collection.Find(filter).ToListAsync();
    }
}