using ApplicationLogic.Commanding.Commands;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.ConversationCommands;

public record MarkConversationAsReadCommand(Guid ConversationId) : IRequest, ICommand;