using MediatR;

namespace ApplicationLogic.Commanding.Commands.MessageCommands;

public record SendMessageToHostCommand(Guid ConversationId, string MessageContent) : ICommand, IRequest;