using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.HostCommands;

public record DeleteHostCommand(Guid Id) : IRequest, ICommand;