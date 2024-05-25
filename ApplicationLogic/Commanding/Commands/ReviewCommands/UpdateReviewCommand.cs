using ApplicationCommon.DTOs.Review;
using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.ReviewCommands;

public record UpdateReviewCommand(Guid Id, ReviewUpdateDTO Review) : IRequest, ICommand;