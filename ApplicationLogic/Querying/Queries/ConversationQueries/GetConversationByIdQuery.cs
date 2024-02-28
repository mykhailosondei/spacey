using ApplicationDAL.Entities;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ConversationQueries;

public record GetConversationByIdQuery(Guid Id) : IRequest<Conversation>;