using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Querying.Queries.ConversationQueries;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ConversationHandlers;

public class GetConversationsQueryHandler : BaseHandler, IRequestHandler<GetConversationsQuery, IEnumerable<Conversation>>
{
    private readonly IConversationQueryRepository _conversationQueryRepository;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IHostIdGetter _hostIdGetter;
    
    public GetConversationsQueryHandler(IMapper mapper, IHostIdGetter hostIdGetter, IUserIdGetter userIdGetter, IConversationQueryRepository conversationQueryRepository) : base(mapper)
    {
        _hostIdGetter = hostIdGetter;
        _userIdGetter = userIdGetter;
        _conversationQueryRepository = conversationQueryRepository;
    }

    public async Task<IEnumerable<Conversation>> Handle(GetConversationsQuery query, CancellationToken cancellationToken)
    {
        var isByUserId = query.Request.UserId is not null;
        var isByHostId = query.Request.HostId is not null;
        
        
        Guid userId;
        Guid hostId;

        if (isByHostId && isByUserId)
        {
            userId = query.Request.UserId!.Value;
            hostId = query.Request.HostId!.Value;

            if (userId != _userIdGetter.UserId && hostId != _hostIdGetter.HostId)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action");
            }
            return await _conversationQueryRepository.GetConversationsByUserIdAndHostId(userId, hostId);
        }

        if (isByUserId)
        {
            userId = query.Request.UserId!.Value;
                
            if (userId != _userIdGetter.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action");
            }
                
            return await _conversationQueryRepository.GetConversationsByUserId(userId);
        }
        
        hostId = query.Request.HostId!.Value;
            
        if (hostId != _hostIdGetter.HostId)
        {
            throw new UnauthorizedAccessException("You are not authorized to perform this action");
        }
            
        return await _conversationQueryRepository.GetConversationsByHostId(hostId);
    }
}