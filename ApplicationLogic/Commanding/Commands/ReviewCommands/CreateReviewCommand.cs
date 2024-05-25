using ApplicationCommon.DTOs.Review;
using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.ReviewCommands;

public record CreateReviewCommand(ReviewCreateDTO Review) : IRequest<Guid>, ICommand;