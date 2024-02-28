using MediatR;

namespace ApplicationLogic.Commanding.Commands.ConversationCommands;

public record DeleteConversationCommand(Guid Id) : IRequest, ICommand;