using ApplicationDAL.Entities;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ConversationQueries;

public record GetConversationByIdQuery(Guid Id) : IRequest<Conversation>;