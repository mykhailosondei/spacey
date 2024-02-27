using ApplicationDAL.Entities;

namespace ApplicationDAL.Interfaces.QueryRepositories;

public interface IConversationQueryRepository
{
    Task<Conversation> GetConversationById(Guid conversationId);
    Task<List<Conversation>> GetConversationsByUserId(Guid userId);
    Task<List<Conversation>> GetConversationsByHostId(Guid hostId);
    Task<List<Conversation>> GetConversationsByBookingId(Guid bookingId);
    Task<List<Message>> GetMessagesByConversationId(Guid conversationId);
    Task<Message> GetMessageById(Guid messageId);
    Task<Conversation> GetConversationByUserIdAndBookingId(Guid userId, Guid bookingId);
    Task<List<Conversation>> GetConversationsByUserIdAndHostId(Guid userId, Guid hostId);
    Task<List<Conversation>> GetConversationsByHostIdAndBookingId(Guid hostId, Guid bookingId);
}