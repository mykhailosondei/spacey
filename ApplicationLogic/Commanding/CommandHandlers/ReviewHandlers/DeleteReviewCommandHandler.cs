using ApplicationCommon.DTOs.Review;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ReviewCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ReviewHandlers;

public class DeleteReviewCommandHandler : BaseHandler,IRequestHandler<DeleteReviewCommand>
{
    private readonly IReviewCommandAccess _reviewCommandAccess;
    private readonly IReviewQueryRepository _reviewQueryRepository;
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IListingCommandAccess _listingCommandAccess;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IPublisher _publisher;
    private readonly IBookingQueryRepository _bookingQueryRepository;

    public DeleteReviewCommandHandler(IMapper mapper, IReviewCommandAccess reviewCommandAccess, IUserIdGetter userIdGetter, IReviewQueryRepository reviewQueryRepository, IPublisher publisher, IListingQueryRepository listingQueryRepository, IListingCommandAccess listingCommandAccess, IBookingQueryRepository bookingQueryRepository) : base(mapper)
    {
        _reviewCommandAccess = reviewCommandAccess;
        _userIdGetter = userIdGetter;
        _reviewQueryRepository = reviewQueryRepository;
        _publisher = publisher;
        _listingQueryRepository = listingQueryRepository;
        _listingCommandAccess = listingCommandAccess;
        _bookingQueryRepository = bookingQueryRepository;
    }

    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewQueryRepository.GetReviewById(request.Id);
        
        var booking = await _bookingQueryRepository.GetBookingById(review.BookingId);

        var listing = await _listingQueryRepository.GetListingById(booking.ListingId);
        
        if (review == null)
        {
            throw new NotFoundException("Review");
        }
        
        if (review.UserId != _userIdGetter.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this review.");
        }

        if (booking == null)
        {
            throw new NotFoundException("Booking");
        }
        
        if (listing == null)
        {
            throw new NotFoundException("Listing");
        }
        
        listing.Ratings.Remove(_mapper.Map<ReviewDTO>(review).Ratings);
        
        await _listingCommandAccess.UpdateListing(listing.Id, listing);
        
        await _reviewCommandAccess.DeleteReview(request.Id);
        
        await _publisher.Publish(new ReviewDeletedEvent()
        {
            ReviewId = review.Id,
            UserId = review.UserId,
            BookingId = review.BookingId,
            DeletedAt = DateTime.UtcNow
        });
    }
}