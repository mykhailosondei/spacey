using ApplicationCommon.DTOs.Review;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using ApplicationLogic.Commanding.Commands.ReviewCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Helpers;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ReviewHandlers;

public class CreateReviewCommandHandler :BaseHandler, IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IReviewCommandAccess _reviewCommandAccess;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IListingCommandAccess _listingCommandAccess;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IPublisher _publisher;
    
    
    public CreateReviewCommandHandler(IMapper mapper, IReviewCommandAccess reviewCommandAccess, IBookingQueryRepository bookingQueryRepository, IUserIdGetter userIdGetter, IListingQueryRepository listingQueryRepository, IListingCommandAccess listingCommandAccess, IPublisher publisher) : base(mapper)
    {
        _reviewCommandAccess = reviewCommandAccess;
        _bookingQueryRepository = bookingQueryRepository;
        _userIdGetter = userIdGetter;
        _listingQueryRepository = listingQueryRepository;
        _listingCommandAccess = listingCommandAccess;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var reviewDTO = _mapper.Map<ReviewDTO>(request.Review);
        var review = _mapper.Map<Review>(reviewDTO);
        
        var booking = await _bookingQueryRepository.GetBookingById(request.Review.BookingId);

        var listing = await _listingQueryRepository.GetListingById(booking.ListingId);
        
        if (booking == null)
        {
            throw new NotFoundException("Booking");
        }
        
        if (booking.UserId != _userIdGetter.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to create a review for this booking.");
        }
        
        if(booking.Review != null)
        {
            throw new InvalidOperationException("This booking already has a review, use the update command instead.");
        }
        
        review.CreatedAt = DateTime.UtcNow;

        listing.Ratings.Add(request.Review.Ratings);
        
        await _listingCommandAccess.UpdateListing(listing.Id, listing);
        
        await _publisher.Publish(new ReviewCreatedEvent()
        {
            ReviewId = review.Id,
            UserId = review.UserId,
            BookingId = booking.Id,
            CreatedAt = DateTime.UtcNow
        });
        
        return await _reviewCommandAccess.AddReview(review);
    }
}