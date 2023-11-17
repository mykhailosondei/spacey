using ApplicationCommon.DTOs.Review;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ReviewQueries;

public record GetReviewByIdQuery(Guid Id) : IRequest<ReviewDTO>;