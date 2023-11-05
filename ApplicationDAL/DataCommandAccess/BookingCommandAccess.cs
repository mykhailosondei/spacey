using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class BookingCommandAccess : BaseAccessHandler
{
    private readonly IMongoCollection<Booking> _collection = GetCollection<Booking>("bookings");
    
    public async Task AddBooking(Booking booking)
    {
        booking.Id = Guid.NewGuid();
        await _collection.InsertOneAsync(booking);
    }
    
    public async Task UpdateBooking(Guid id, Booking booking)
    {
        var filter = Builders<Booking>.Filter.Eq("Id", id);
        booking.Id = id;
        await _collection.ReplaceOneAsync(filter, booking);
    }
    
    public async Task DeleteBooking(Guid id)
    {
        var filter = Builders<Booking>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
    }
}