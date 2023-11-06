using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class BookingCommandAccess : BaseAccessHandler
{
    private readonly IMongoCollection<Booking> _collection = GetCollection<Booking>("bookings");
    private readonly IReviewDeletor _reviewDeletor;

    public BookingCommandAccess(IReviewDeletor reviewDeletor)
    {
        _reviewDeletor = reviewDeletor;
    }

    public async Task<Guid> AddBooking(Booking booking)
    {
        booking.Id = Guid.NewGuid();
        await _collection.InsertOneAsync(booking);
        return booking.Id;
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
        await UpdateListingBookingsIdsOnBookingDelete(id);
        await UpdateUserBookingsIdsOnBookingDelete(id);
        var reviewFilter = Builders<Review>.Filter.Eq("BookingId", id);
        Guid reviewId = (await GetCollection<Review>("reviews").Find(reviewFilter).FirstOrDefaultAsync()).Id;
        await _reviewDeletor.DeleteReview(reviewId);
    }
    
    private async Task UpdateListingBookingsIdsOnBookingDelete(Guid id)
    {
        var listingFilter = Builders<Listing>.Filter.In("BookingsIds", new[]{id});
        var listingUpdate = Builders<Listing>.Update.Pull("BookingsIds", id);
        await GetCollection<Listing>("listings").UpdateOneAsync(listingFilter, listingUpdate);
    }
    
    private async Task UpdateUserBookingsIdsOnBookingDelete(Guid id)
    {
        var userFilter = Builders<User>.Filter.In("BookingsIds", new[]{id});
        var userUpdate = Builders<User>.Update.Pull("BookingsIds", id);
        await GetCollection<User>("users").UpdateOneAsync(userFilter, userUpdate);
    }
}