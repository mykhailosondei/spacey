using ApplicationCommon.DTOs.Review;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ReviewQueries;

public record GetReviewByIdQuery(Guid Id) : IRequest<ReviewDTO>;