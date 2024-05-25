using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.ReviewCommands;

public record DeleteReviewCommand(Guid Id) : IRequest, ICommand;