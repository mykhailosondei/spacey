using ApplicationCommon.DTOs.Review;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Entities;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ReviewCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ReviewHandlers;

public class CreateReviewCommandHandler :BaseHandler, IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IReviewCommandAccess _reviewCommandAccess;
    
    public CreateReviewCommandHandler(IMapper mapper, IReviewCommandAccess reviewCommandAccess) : base(mapper)
    {
        _reviewCommandAccess = reviewCommandAccess;
    }

    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var reviewDTO = _mapper.Map<ReviewDTO>(request.Review);
        var review = _mapper.Map<Review>(reviewDTO);
        
        return await _reviewCommandAccess.AddReview(review);
    }
}