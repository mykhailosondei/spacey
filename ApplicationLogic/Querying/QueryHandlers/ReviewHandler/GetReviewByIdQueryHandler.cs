using ApplicationCommon.DTOs.Review;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.ReviewQueries;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

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
        var result = await _reviewQueryRepository.GetReviewById(request.Id);
        
        if (result == null)
        {
            throw new NotFoundException(nameof(ReviewDTO));
        }
        
        return _mapper.Map<ReviewDTO>(result);
    }
}