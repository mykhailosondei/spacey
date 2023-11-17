using ApplicationCommon.DTOs.Review;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ReviewQueries;

public record GetAllReviewsQuery() : IRequest<IEnumerable<ReviewDTO>>;