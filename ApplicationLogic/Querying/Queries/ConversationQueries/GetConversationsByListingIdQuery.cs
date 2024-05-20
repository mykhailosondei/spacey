using ApplicationDAL.Entities;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ConversationQueries;

public record GetConversationsByListingIdQuery(Guid ListingId) : IRequest<IEnumerable<Conversation>>;