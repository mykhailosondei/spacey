using ApplicationCommon.DTOs.Review;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.ReviewQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ReviewHandler;

public class GetAllReviewsQueryHandler : BaseHandler, IRequestHandler<GetAllReviewsQuery, IEnumerable<ReviewDTO>>
{
    private readonly IReviewQueryRepository _reviewQueryRepository;
    
    public GetAllReviewsQueryHandler(IMapper mapper, IReviewQueryRepository reviewQueryRepository) : base(mapper)
    {
        _reviewQueryRepository = reviewQueryRepository;
    }

    public async Task<IEnumerable<ReviewDTO>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
    {
        var result = await _reviewQueryRepository.GetAllReviews();

        if (!result.Any())
        {
            throw new NotFoundException(nameof(IEnumerable<ReviewDTO>));
        }
        
        return _mapper.Map<IEnumerable<ReviewDTO>>(result);
    }
}