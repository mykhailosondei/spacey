namespace ApplicationDAL.Interfaces.CommandAccess;

public interface IConversationCommandAccess
{
    Task SendMessageToUser(Guid conversationId, Guid hostId, string messageContent);
    Task SendMessageToHost(Guid conversationId, Guid userId, string messageContent);
    Task<Guid> CreateConversation(Guid userId, Guid hostId, Guid bookingId);
    Task DeleteConversation(Guid conversationId);
}