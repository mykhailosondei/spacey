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
    
    public DeleteReviewCommandHandler(IMapper mapper, IReviewCommandAccess reviewCommandAccess, IUserIdGetter userIdGetter, IReviewQueryRepository reviewQueryRepository) : base(mapper)
    {
        _reviewCommandAccess = reviewCommandAccess;
        _userIdGetter = userIdGetter;
        _reviewQueryRepository = reviewQueryRepository;
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
    }
}