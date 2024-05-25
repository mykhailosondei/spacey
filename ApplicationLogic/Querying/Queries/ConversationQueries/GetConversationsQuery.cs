using ApplicationCommon.Structs;
using ApplicationDAL.Entities;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ConversationQueries;

public record GetConversationsQuery(ConversationsRequest Request) : IRequest<IEnumerable<Conversation>>;