using ApplicationCommon.DTOs.Review;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Querying.Queries.ReviewQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ReviewHandler;

public class GetReviewByIdQueryHandler : BaseHandler, IRequestHandler<GetReviewByIdQuery, ReviewDTO>
{
    private readonly IReviewQueryRepository _reviewQueryRepository;
    
    public GetReviewByIdQueryHandler(IMapper mapper, IReviewQueryRepository reviewQueryRepository) : base(mapper)
    {
        _reviewQueryRepository = reviewQueryRepository;
    }

    public async Task<ReviewDTO> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<ReviewDTO>(await _reviewQueryRepository.GetReviewById(request.Id));
    }
}