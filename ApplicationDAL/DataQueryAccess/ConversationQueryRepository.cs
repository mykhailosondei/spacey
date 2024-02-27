using ApplicationDAL.DataQueryAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using MongoDB.Driver;

namespace ApplicationDAL.DataQueryAccess;

public class ConversationQueryRepository : BaseQueryRepository, IConversationQueryRepository
{

    private readonly IMongoCollection<Conversation> _collection = GetCollection<Conversation>("conversations");
    
    public async Task<Conversation> GetConversationById(Guid conversationId)
    {
        var filter = Builders<Conversation>.Filter.Eq("Id", conversationId);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<List<Conversation>> GetConversationsByUserId(Guid userId)
    {
        var filter = Builders<Conversation>.Filter.Eq("UserId", userId);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<List<Conversation>> GetConversationsByHostId(Guid hostId)
    {
        var filter = Builders<Conversation>.Filter.Eq("HostId", hostId);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<List<Conversation>> GetConversationsByBookingId(Guid bookingId)
    {
        var filter = Builders<Conversation>.Filter.Eq("BookingId", bookingId);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<List<Message>> GetMessagesByConversationId(Guid conversationId)
    { 
        var filter = Builders<Conversation>.Filter.Eq("Id", conversationId);
        var result = await _collection.Find(filter).SingleOrDefaultAsync();
        return result.Messages;
    }

    public async Task<Message> GetMessageById(Guid messageId)
    {
        var filter = Builders<Conversation>.Filter.ElemMatch("Messages", Builders<Message>.Filter.Eq("Id", messageId));
        var result = await _collection.Find(filter).SingleOrDefaultAsync();
        return result.Messages.Find(x => x.Id == messageId)!;
    }

    public async Task<Conversation> GetConversationByUserIdAndBookingId(Guid userId, Guid bookingId)
    {
        var filter = Builders<Conversation>.Filter.And(
            Builders<Conversation>.Filter.Eq("UserId", userId),
            Builders<Conversation>.Filter.Eq("BookingId", bookingId)
        );
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<Conversation> GetConversationByUserIdAndHostIdAndBookingId(Guid userId, Guid hostId, Guid bookingId)
    {
        var filter = Builders<Conversation>.Filter.And(
            Builders<Conversation>.Filter.Eq("UserId", userId),
            Builders<Conversation>.Filter.Eq("HostId", hostId),
            Builders<Conversation>.Filter.Eq("BookingId", bookingId)
        );
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<List<Conversation>> GetConversationsByUserIdAndHostId(Guid userId, Guid hostId)
    {
        var filter = Builders<Conversation>.Filter.And(
            Builders<Conversation>.Filter.Eq("UserId", userId),
            Builders<Conversation>.Filter.Eq("HostId", hostId)
        );
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<List<Conversation>> GetConversationsByHostIdAndBookingId(Guid hostId, Guid bookingId)
    {
        var filter = Builders<Conversation>.Filter.And(
            Builders<Conversation>.Filter.Eq("HostId", hostId),
            Builders<Conversation>.Filter.Eq("BookingId", bookingId)
        );
        return await _collection.Find(filter).ToListAsync();
    }
}