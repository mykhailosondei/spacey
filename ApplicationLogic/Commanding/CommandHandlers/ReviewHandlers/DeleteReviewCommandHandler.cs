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
    private readonly IUserIdGetter _userIdGetter;
    private readonly IPublisher _publisher;
    
    public DeleteReviewCommandHandler(IMapper mapper, IReviewCommandAccess reviewCommandAccess, IUserIdGetter userIdGetter, IReviewQueryRepository reviewQueryRepository, IPublisher publisher) : base(mapper)
    {
        _reviewCommandAccess = reviewCommandAccess;
        _userIdGetter = userIdGetter;
        _reviewQueryRepository = reviewQueryRepository;
        _publisher = publisher;
    }

    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewQueryRepository.GetReviewById(request.Id);
        
        if (review == null)
        {
            throw new NotFoundException("Review");
        }
        
        if (review.UserId != _userIdGetter.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this review.");
        }
        
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