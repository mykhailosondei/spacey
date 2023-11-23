using System.Reflection;
using ApplicationDAL.Utilities;
using ApplicationDAL.Attributes;
using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces;
using ApplicationDAL.Interfaces.CommandAccess;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class BookingCommandAccess : BaseAccessHandler, IBookingDeletor, IBookingCommandAccess
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
        await UpdateListingBookingsIdsOnBookingAdd(booking.ListingId, booking);
        await UpdateUserBookingsIdsOnBookingAdd(booking.UserId, booking);
        return booking.Id;
    }

    private async Task UpdateUserBookingsIdsOnBookingAdd(Guid userId, Booking booking)
    {
        var userFilter = Builders<User>.Filter.Eq("Id", userId);
        var userUpdate = Builders<User>.Update.Push("BookingsIds", booking.Id);
        await GetCollection<User>("users").UpdateOneAsync(userFilter, userUpdate);
    }

    private async Task UpdateListingBookingsIdsOnBookingAdd(Guid id, Booking booking)
    {
        var listingFilter = Builders<Listing>.Filter.Eq("Id", id);
        var listingUpdate = Builders<Listing>.Update.Push("BookingsIds", booking.Id);
        await GetCollection<Listing>("listings").UpdateOneAsync(listingFilter, listingUpdate);
    }
        
    public async Task UpdateBooking(Guid id, Booking booking)
    {
        var filter = Builders<Booking>.Filter.Eq("Id", id);
        booking.Id = id;
        var documentToSet = booking.ToBsonDocument();
        foreach (var name in ReflectionUtilities.GetPropertyNamesThatAreMarkedWithAttribute<RestrictUpdateAttribute>(typeof(Booking)))
        {
            Console.WriteLine(name);
            documentToSet.Remove(name);
        }
        var update = new BsonDocument("$set", documentToSet);
        await _collection.UpdateOneAsync(filter, update);
    }
    
    public async Task DeleteBooking(Guid id)
    {
        var filter = Builders<Booking>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
        await UpdateListingBookingsIdsOnBookingDelete(id);
        await UpdateUserBookingsIdsOnBookingDelete(id);
        var reviewFilter = Builders<Review>.Filter.Eq("BookingId", id);
        Guid? reviewId = (await GetCollection<Review>("reviews").Find(reviewFilter).FirstOrDefaultAsync())?.Id;
        if(reviewId != null) await _reviewDeletor.DeleteReview(reviewId.Value);
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