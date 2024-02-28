using ApplicationCommon.Structs;
using ApplicationDAL.Entities;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ConversationQueries;

public record GetConversationsQuery(ConversationsRequest Request) : IRequest<IEnumerable<Conversation>>;