using ApplicationDAL.Entities;

namespace ApplicationDAL.Interfaces.CommandAccess;

public interface IBookingCommandAccess
{
    public Task<Guid> AddBooking(Booking booking);

    public Task UpdateBooking(Guid id, Booking booking);

    public Task DeleteBooking(Guid id);
}