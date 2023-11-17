using ApplicationCommon.DTOs.Review;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.ReviewCommands;

public record UpdateReviewCommand(Guid Id, ReviewUpdateDTO Review) : IRequest;