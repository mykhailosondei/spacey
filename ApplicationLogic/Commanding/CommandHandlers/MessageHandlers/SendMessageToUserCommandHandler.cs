using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.MessageCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Notifications;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ApplicationLogic.Commanding.CommandHandlers.MessageHandlers;

public class SendMessageToUserCommandHandler : BaseHandler ,IRequestHandler<SendMessageToUserCommand>
{

    private readonly IHostIdGetter _hostIdGetter;
    private readonly IConversationQueryRepository _conversationQueryRepository;
    private readonly IConversationCommandAccess _conversationCommandAccess;
    private readonly IHostQueryRepository _hostQueryRepository;
    private readonly IUserQueryRepository _userQueryRepository; 
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

    public SendMessageToUserCommandHandler(IMapper mapper, IHostIdGetter hostIdGetter, IHostQueryRepository hostQueryRepository, IConversationCommandAccess conversationCommandAccess, IConversationQueryRepository conversationQueryRepository, IHubContext<NotificationHub, INotificationClient> hubContext, IUserQueryRepository userQueryRepository) : base(mapper)
    {
        _hostIdGetter = hostIdGetter;
        _hostQueryRepository = hostQueryRepository;
        _conversationCommandAccess = conversationCommandAccess;
        _conversationQueryRepository = conversationQueryRepository;
        _hubContext = hubContext;
        _userQueryRepository = userQueryRepository;
    }

    public async Task Handle(SendMessageToUserCommand request, CancellationToken cancellationToken)
    {
        var hostId = _hostIdGetter.HostId;
        var host = await _hostQueryRepository.GetHostById(hostId);
        if (host is null)
        {
            throw new NotFoundException("Host");
        }
        var hostUsername = (await _userQueryRepository.GetUserById(host.UserId)).Name;
        
        var conversation = await _conversationQueryRepository.GetConversationById(request.ConversationId);
        if (conversation is null)
        {
            throw new NotFoundException("Conversation");
        }
        
        if (conversation.HostId != hostId)
        {
            throw new UnauthorizedAccessException("You are not allowed to send messages to this conversation");
        }
        
        await _conversationCommandAccess.SendMessageToUser(request.ConversationId, hostId, request.MessageContent);
        
        var receiver = await _userQueryRepository.GetUserById(conversation.UserId);

        if (receiver is null)
        {
            throw new NotFoundException("Receiver");
        }
        
        await _hubContext.Clients.All.ReceiveNotification($"You have a new message from {hostUsername}");
    }
}