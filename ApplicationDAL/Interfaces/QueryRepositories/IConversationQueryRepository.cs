using ApplicationDAL.Entities;

namespace ApplicationDAL.Interfaces.QueryRepositories;

public interface IConversationQueryRepository
{
    Task<Conversation> GetConversationById(Guid conversationId);
    Task<List<Conversation>> GetConversationsByUserId(Guid userId);
    Task<List<Conversation>> GetConversationsByHostId(Guid hostId);
    Task<Conversation> GetConversationByBookingId(Guid bookingId);
    Task<List<Conversation>> GetConversationsByUserIdAndHostId(Guid userId, Guid hostId);
}