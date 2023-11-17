using ApplicationCommon.DTOs.Review;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Entities;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ReviewCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ReviewHandlers;

public class UpdateReviewCommandHandler : BaseHandler, IRequestHandler<UpdateReviewCommand>
{
    private readonly IReviewCommandAccess _reviewCommandAccess;
    
    public UpdateReviewCommandHandler(IMapper mapper, IReviewCommandAccess reviewCommandAccess) : base(mapper)
    {
        _reviewCommandAccess = reviewCommandAccess;
    }

    public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var reviewDTO = _mapper.Map<ReviewDTO>(request.Review);
        var review = _mapper.Map<Review>(reviewDTO);
        
        await _reviewCommandAccess.UpdateReview(request.Id, review);
    }
}