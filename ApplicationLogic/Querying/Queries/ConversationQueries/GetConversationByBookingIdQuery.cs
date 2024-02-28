using ApplicationDAL.Entities;
using MediatR;

namespace ApplicationLogic.Querying.Queries.ConversationQueries;

public record GetConversationByBookingIdQuery(Guid BookingId) : IRequest<Conversation>;