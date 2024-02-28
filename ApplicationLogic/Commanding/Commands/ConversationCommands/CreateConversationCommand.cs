using MediatR;

namespace ApplicationLogic.Commanding.Commands.ConversationCommands;

public record CreateConversationCommand(Guid UserId, Guid HostId, Guid BookingId) : IRequest<Guid>, ICommand;