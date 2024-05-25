using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.UserCommands;

public record DeleteUserCommand(Guid Id) : IRequest, ICommand;