using MediatR;

namespace ApplicationLogic.Commanding.Commands.UserCommands;

public record DeleteUserCommand(Guid Id) : IRequest<Unit>;