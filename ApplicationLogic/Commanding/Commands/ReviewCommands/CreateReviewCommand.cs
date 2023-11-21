using ApplicationCommon.DTOs.Review;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.ReviewCommands;

public record CreateReviewCommand(ReviewCreateDTO Review) : IRequest<Guid>, ICommand;