using MediatR;

namespace ApplicationLogic.Commanding.Commands.ConversationCommands;

public record CreateConversationCommand(Guid BookingId) : IRequest<Guid>, ICommand;