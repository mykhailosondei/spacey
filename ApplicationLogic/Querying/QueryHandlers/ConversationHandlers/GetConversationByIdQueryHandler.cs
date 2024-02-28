using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Querying.Queries.ConversationQueries;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ConversationHandlers;

public class GetConversationByIdQueryHandler : BaseHandler, IRequestHandler<GetConversationByIdQuery, Conversation>
{

    private readonly IConversationQueryRepository _conversationQueryRepository;
    private readonly IHostIdGetter _hostIdGetter;
    private readonly IUserIdGetter _userIdGetter;
    
    public GetConversationByIdQueryHandler(IMapper mapper, IUserIdGetter userIdGetter, IHostIdGetter hostIdGetter, IConversationQueryRepository conversationQueryRepository) : base(mapper)
    {
        _userIdGetter = userIdGetter;
        _hostIdGetter = hostIdGetter;
        _conversationQueryRepository = conversationQueryRepository;
    }

    public async Task<Conversation> Handle(GetConversationByIdQuery request, CancellationToken cancellationToken)
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
        
        return conversation;
    }
}