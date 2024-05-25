using ApplicationDAL.Entities;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.ConversationQueries;

public record GetConversationByBookingIdQuery(Guid BookingId) : IRequest<Conversation>;