using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Commanding.Commands.ConversationCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Notifications;
using ApplicationLogic.UserIdLogic;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ApplicationLogic.Commanding.CommandHandlers.ConversationHandlers;

public class MarkConversationAsReadCommandHandler : IRequestHandler<MarkConversationAsReadCommand>
{

    private readonly IConversationQueryRepository _conversationQueryRepository;
    private readonly IConversationCommandAccess _conversationCommandAccess;
    private readonly IHostIdGetter _hostIdGetter;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IHubContext<NotificationHub, INotificationClient> _notificationHub;

    public MarkConversationAsReadCommandHandler(IConversationQueryRepository conversationQueryRepository, IConversationCommandAccess conversationCommandAccess, IHostIdGetter hostIdGetter, IUserIdGetter userIdGetter, IHubContext<NotificationHub, INotificationClient> notificationHub)
    {
        _conversationQueryRepository = conversationQueryRepository;
        _conversationCommandAccess = conversationCommandAccess;
        _hostIdGetter = hostIdGetter;
        _userIdGetter = userIdGetter;
        _notificationHub = notificationHub;
    }

    public async Task Handle(MarkConversationAsReadCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _conversationQueryRepository.GetConversationById(request.ConversationId);
        if (conversation == null)
        {
            throw new NotFoundException(nameof(Conversation));
        }

        var userId = _userIdGetter.UserId;
        var hostId = _hostIdGetter.HostId;

        if (conversation.HostId != hostId && conversation.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not allowed to mark this conversation as read.");
        }

        if (conversation.IsRead)
        {
            return;
        }

        var isHost = hostId == conversation.HostId;
        
        if (isHost)
        {
            if(conversation.Messages.Last().UserId == null)
            {
                throw new UnauthorizedAccessException("You are not allowed to mark this conversation as read.");
            }
        }
        else
        {
            if (conversation.Messages.Last().HostId == null)
            {
                throw new UnauthorizedAccessException("You are not allowed to mark this conversation as read.");
            }
        }
        
        conversation.IsRead = true;
        await _conversationCommandAccess.UpdateConversation(conversation.Id, conversation);
        await _notificationHub.Clients.User(conversation.UserId.ToString()).ReadNotification(conversation.Id.ToString());
        await _notificationHub.Clients.User(conversation.HostId.ToString()).ReadNotification(conversation.Id.ToString());
    }
}