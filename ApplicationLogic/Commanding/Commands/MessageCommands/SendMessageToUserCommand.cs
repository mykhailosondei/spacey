using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.MessageCommands;

public record SendMessageToUserCommand(Guid ConversationId, string MessageContent) : ICommand, IRequest;