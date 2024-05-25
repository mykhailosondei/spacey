using ApplicationCommon.DTOs.Review;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ReviewQueries;

public record GetAllReviewsQuery() : IRequest<IEnumerable<ReviewDTO>>;