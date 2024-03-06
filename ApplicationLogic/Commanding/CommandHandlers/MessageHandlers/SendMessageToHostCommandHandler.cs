using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.MessageCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Notifications;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver.Core.Misc;

namespace ApplicationLogic.Commanding.CommandHandlers.MessageHandlers;

public class SendMessageToHostCommandHandler : BaseHandler, IRequestHandler<SendMessageToHostCommand>
{
    
    private readonly IUserIdGetter _userIdGetter;
    private readonly IConversationQueryRepository _conversationQueryRepository;
    private readonly IConversationCommandAccess _conversationCommandAccess;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IHostQueryRepository _hostQueryRepository;
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
    
    public SendMessageToHostCommandHandler(IMapper mapper, IUserIdGetter userIdGetter, IConversationQueryRepository conversationQueryRepository, IConversationCommandAccess conversationCommandAccess, IUserQueryRepository userQueryRepository, IHubContext<NotificationHub, INotificationClient> hubContext, IHostQueryRepository hostQueryRepository) : base(mapper)
    {
        _userIdGetter = userIdGetter;
        _conversationQueryRepository = conversationQueryRepository;
        _conversationCommandAccess = conversationCommandAccess;
        _userQueryRepository = userQueryRepository;
        _hubContext = hubContext;
        _hostQueryRepository = hostQueryRepository;
    }
    
    
    public async Task Handle(SendMessageToHostCommand request, CancellationToken cancellationToken)
    {
        var userId = _userIdGetter.UserId;

        var user = await _userQueryRepository.GetUserById(userId);
        if (user is null)
        {
            throw new NotFoundException("User");
        }
        
        var conversation = await _conversationQueryRepository.GetConversationById(request.ConversationId);
        
        if (conversation is null)
        {
            throw new NotFoundException("Conversation");
        }
        
        if (conversation.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not allowed to send messages to this conversation");
        }
        
        var host = await _hostQueryRepository.GetHostById(conversation.HostId);
        
        if (host is null)
        {
            throw new NotFoundException("Host");
        }
        
        conversation.IsRead = false;
        
        await _conversationCommandAccess.UpdateConversation(conversation.Id, conversation);
        
        await _conversationCommandAccess.SendMessageToHost(request.ConversationId, userId, request.MessageContent);
        
        await _hubContext.Clients.User(conversation.HostId.ToString()).ReceiveNotification(conversation.Id.ToString());
    }

}