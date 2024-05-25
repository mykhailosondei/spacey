using ApplicationDAL.Entities;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ConversationQueries;

public record GetConversationsByListingIdQuery(Guid ListingId) : IRequest<IEnumerable<Conversation>>;