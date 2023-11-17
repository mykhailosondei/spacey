using ApplicationDAL.DataCommandAccess;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ReviewCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ReviewHandlers;

public class DeleteReviewCommandHandler : BaseHandler,IRequestHandler<DeleteReviewCommand>
{
    private readonly IReviewCommandAccess _reviewCommandAccess;
    
    public DeleteReviewCommandHandler(IMapper mapper, IReviewCommandAccess reviewCommandAccess) : base(mapper)
    {
        _reviewCommandAccess = reviewCommandAccess;
    }

    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        await _reviewCommandAccess.DeleteReview(request.Id);
    }
}