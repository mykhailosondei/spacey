using ApplicationCommon.DTOs.Host;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.HostCommands;

public record UpdateHostCommand(Guid Id, HostUpdateDTO Host) : IRequest<Unit>;