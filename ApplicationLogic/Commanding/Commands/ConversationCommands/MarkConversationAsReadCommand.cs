using ApplicationLogic.Commanding.Commands;
using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.ConversationCommands;

public record MarkConversationAsReadCommand(Guid ConversationId) : IRequest, ICommand;