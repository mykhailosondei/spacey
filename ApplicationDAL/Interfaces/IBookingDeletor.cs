namespace ApplicationDAL.Interfaces;

public interface IBookingDeletor
{
    public Task DeleteBooking(Guid id);
}