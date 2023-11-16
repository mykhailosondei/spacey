
using ApplicationDAL.Utilities;
using ApplicationDAL.Attributes;
using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class ReviewCommandAccess : BaseAccessHandler, IReviewDeletor, IReviewCommandAccess
{
    private readonly IMongoCollection<Review> _collection = GetCollection<Review>("reviews");
    
    public async Task<Guid> AddReview(Review review)
    {
        review.Id = Guid.NewGuid();
        await _collection.InsertOneAsync(review);
        await UpdateUserReviewsIdsOnReviewAdd(review.UserId, review);
        await UpdateBookingReviewOnReviewAdd(review.BookingId, review);
        return review.Id;
    }
    
    private async Task UpdateBookingReviewOnReviewAdd(Guid id, Review review)
    {
        var bookingFilter = Builders<Booking>.Filter.Eq("Id", id);
        var bookingUpdate = Builders<Booking>.Update.Set("Review", review);
        await GetCollection<Booking>("bookings").UpdateOneAsync(bookingFilter, bookingUpdate);
    }
    
    private async Task UpdateUserReviewsIdsOnReviewAdd(Guid id, Review review)
    {
        var userFilter = Builders<User>.Filter.Eq("Id", id);
        var userUpdate = Builders<User>.Update.Push("ReviewsIds", review.Id);
        await GetCollection<User>("users").UpdateOneAsync(userFilter, userUpdate);
    }
    
    public async Task UpdateReview(Guid id, Review review)
    {
        var filter = Builders<Review>.Filter.Eq("Id", id);
        review.Id = id;
        var documentToSet = review.ToBsonDocument();
        foreach (var name in ReflectionUtilities.GetPropertyNamesThatAreMarkedWithAttribute<RestrictUpdateAttribute>(typeof(Review)))
        {
            Console.WriteLine(name);
            documentToSet.Remove(name);
        }
        var update = new BsonDocument("$set", documentToSet);await _collection.UpdateOneAsync(filter, update);
        await UpdateBookingReviewOnReviewUpdate(id, review);
    }
    
    private async Task UpdateBookingReviewOnReviewUpdate(Guid id, Review review)
    {
        var bookingFilter = Builders<Booking>.Filter.Eq("Review.Id", id);
        var bookingUpdate = Builders<Booking>.Update.Set("Review", review);
        await GetCollection<Booking>("bookings").UpdateOneAsync(bookingFilter, bookingUpdate);
    }
    
    public async Task DeleteReview(Guid id)
    {
        var filter = Builders<Review>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
        await DeleteBookingReviewOnReviewDelete(id);
        await UpdateUserReviewsIdsOnReviewDelete(id);
        await UpdateListingReviewsIdsOnReviewDelete(id);
    }
    
    private async Task UpdateListingReviewsIdsOnReviewDelete(Guid id)
    {
        var listingFilter = Builders<Listing>.Filter.In("ReviewsIds", new[]{id});
        var listingUpdate = Builders<Listing>.Update.Pull("ReviewsIds", id);
        await GetCollection<Listing>("listings").UpdateOneAsync(listingFilter, listingUpdate);
    }
    
    private async Task UpdateUserReviewsIdsOnReviewDelete(Guid id)
    {
        var userFilter = Builders<User>.Filter.In("ReviewsIds", new[]{id});
        var userUpdate = Builders<User>.Update.Pull("ReviewsIds", id);
        await GetCollection<User>("users").UpdateOneAsync(userFilter, userUpdate);
    }

    private async Task DeleteBookingReviewOnReviewDelete(Guid id)
    {
        var bookingFilter = Builders<Booking>.Filter.Eq("Review.Id", id);
        var bookingUpdate = Builders<Booking>.Update.Set("Review", (Review?) null);
        await GetCollection<Booking>("bookings").UpdateOneAsync(bookingFilter, bookingUpdate);
    }
}