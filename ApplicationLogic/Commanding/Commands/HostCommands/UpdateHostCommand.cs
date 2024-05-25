using ApplicationCommon.DTOs.Host;
using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.HostCommands;

public record UpdateHostCommand(Guid Id, HostUpdateDTO Host) : IRequest, ICommand;