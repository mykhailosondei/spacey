using ApplicationCommon.DTOs.Review;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ReviewCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Helpers;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ReviewHandlers;

public class UpdateReviewCommandHandler : BaseHandler, IRequestHandler<UpdateReviewCommand>
{
    private readonly IReviewCommandAccess _reviewCommandAccess;
    private readonly IReviewQueryRepository _reviewQueryRepository;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IListingCommandAccess _listingCommandAccess;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IPublisher _publisher;
    
    public UpdateReviewCommandHandler(IMapper mapper, IReviewCommandAccess reviewCommandAccess, IReviewQueryRepository reviewQueryRepository, IUserIdGetter userIdGetter, IListingCommandAccess listingCommandAccess, IListingQueryRepository listingQueryRepository, IBookingQueryRepository bookingQueryRepository, IPublisher publisher) : base(mapper)
    {
        _reviewCommandAccess = reviewCommandAccess;
        _reviewQueryRepository = reviewQueryRepository;
        _userIdGetter = userIdGetter;
        _listingCommandAccess = listingCommandAccess;
        _listingQueryRepository = listingQueryRepository;
        _bookingQueryRepository = bookingQueryRepository;
        _publisher = publisher;
    }

    public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var reviewDTO = _mapper.Map<ReviewDTO>(request.Review);
        var review = _mapper.Map<Review>(reviewDTO);
        
        var existingReview = await _reviewQueryRepository.GetReviewById(request.Id);

        if (existingReview == null)
        {
            throw new NotFoundException("Review");
        }
        
        if (existingReview.UserId != _userIdGetter.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this review.");
        }
        
        var booking = await _bookingQueryRepository.GetBookingById(existingReview.BookingId);
        
        var listing = await _listingQueryRepository.GetListingById(booking.ListingId);
        
        listing.Ratings = BookingHelper.CalculateNewRating(listing.BookingsIds.Count,listing.Ratings, request.Review.Ratings.getRatingsArray());
        
        await _listingCommandAccess.UpdateListing(listing.Id, listing);
        
        await _reviewCommandAccess.UpdateReview(request.Id, review);
        
        await _publisher.Publish(new ReviewUpdatedEvent()
        {
            ReviewId = review.Id,
            UserId = review.UserId,
            BookingId = review.BookingId,
            UpdatedAt = DateTime.UtcNow
        });
    }
}