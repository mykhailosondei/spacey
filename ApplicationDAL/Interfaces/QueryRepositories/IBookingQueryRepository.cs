using ApplicationDAL.Entities;

namespace ApplicationDAL.Interfaces.QueryRepositories;

public interface IBookingQueryRepository
{
    public Task<Booking> GetBookingById(Guid id);
    
    public Task<IEnumerable<Booking>> GetAllBookings();
    
    public Task<IEnumerable<Booking>> GetBookingsByUserId(Guid userId);
    
    public Task<IEnumerable<Booking>> GetBookingsByListingId(Guid listingId);
}