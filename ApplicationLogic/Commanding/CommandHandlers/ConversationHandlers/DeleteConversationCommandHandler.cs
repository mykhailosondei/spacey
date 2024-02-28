using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ConversationCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ConversationHandlers;

public class DeleteConversationCommandHandler : BaseHandler ,IRequestHandler<DeleteConversationCommand>
{
    private readonly IConversationCommandAccess _conversationCommandAccess;
    private readonly IConversationQueryRepository _conversationQueryRepository;
    private readonly IHostIdGetter _hostIdGetter;
    private readonly IUserIdGetter _userIdGetter;
    
    public DeleteConversationCommandHandler(IMapper mapper, IUserIdGetter userIdGetter, IHostIdGetter hostIdGetter, IConversationQueryRepository conversationQueryRepository, IConversationCommandAccess conversationCommandAccess) : base(mapper)
    {
        _userIdGetter = userIdGetter;
        _hostIdGetter = hostIdGetter;
        _conversationQueryRepository = conversationQueryRepository;
        _conversationCommandAccess = conversationCommandAccess;
    }

    public async Task Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _conversationQueryRepository.GetConversationById(request.Id);
        
        if (conversation is null)
        {
            throw new NotFoundException("Conversation");
        }
        
        if (conversation.UserId != _userIdGetter.UserId && conversation.HostId != _hostIdGetter.HostId)
        {
            throw new UnauthorizedAccessException("You are not authorized to perform this action");
        }
        
        await _conversationCommandAccess.DeleteConversation(request.Id);
    }
}