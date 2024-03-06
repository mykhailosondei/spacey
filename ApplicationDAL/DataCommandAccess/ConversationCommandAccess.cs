using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class ConversationCommandAccess : BaseAccessHandler, IConversationCommandAccess
{
    private readonly IMongoCollection<Conversation> _collection = GetCollection<Conversation>("conversations");
    
    public async Task<Guid> CreateConversation(Guid userId, Guid hostId, Guid bookingId)
    {
        var conversation = new Conversation()
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            HostId = hostId,
            UserId = userId,
            CreatedAt = DateTime.Now,
            Messages = new()
        };
        await _collection.InsertOneAsync(conversation);
        return conversation.Id;
    }
    
    public async Task SendMessageToUser(Guid conversationId, Guid hostId, string messageContent)
    {
        var message = new Message()
        {
            Id = Guid.NewGuid(),
            Content = messageContent,
            CreatedAt = DateTime.Now,
            HostId = hostId
        };
        var filter = Builders<Conversation>.Filter.Eq("Id", conversationId);
        var update = Builders<Conversation>.Update.Push("Messages", message);
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task SendMessageToHost(Guid conversationId, Guid userId, string messageContent)
    {
        var message = new Message()
        {
            Id = Guid.NewGuid(),
            Content = messageContent,
            CreatedAt = DateTime.Now,
            UserId = userId
        };
        var filter = Builders<Conversation>.Filter.Eq("Id", conversationId);
        var update = Builders<Conversation>.Update.Push("Messages", message);
        await _collection.UpdateOneAsync(filter, update);
    }


    public async Task DeleteConversation(Guid conversationId)
    {
        var filter = Builders<Conversation>.Filter.Eq("Id", conversationId);
        await _collection.DeleteOneAsync(filter);
    }

    public Task UpdateConversation(Guid id, Conversation conversation)
    {
        var filter = Builders<Conversation>.Filter.Eq("Id", id);
        return _collection.ReplaceOneAsync(filter, conversation);
    }
}