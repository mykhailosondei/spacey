using ApplicationCommon.DTOs.Host;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.HostCommands;

public record CreateHostCommand(HostCreateDTO Host) : IRequest<Guid>;