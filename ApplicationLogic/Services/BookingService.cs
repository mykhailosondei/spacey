using ApplicationDataAccess.DataAccess;
using ApplicationDataAccess.Models;

namespace ApplicationLogic.Services;

public class BookingService : BaseService
{
    public BookingService(DataAccessManager dataAccessManager) : base(dataAccessManager)
    {
        
    }
    
    public async Task<List<BookingModel>> GetAllBookings()
    {
        return await _dataAccessManager.GetAllBookings();
    }
        
    
    public async Task<BookingModel> GetBooking(string id)
    {
        return await _dataAccessManager.GetBooking(id);
    }

    
    public async Task CreateBookingAsync( BookingModel model)
    {
        await _dataAccessManager.CreateBookingAsync(model);
    }

    
    public async Task UpdateBookingAsync(string id, BookingModel model)
    {
        await _dataAccessManager.UpdateBookingAsync(id, model);
    }

    
    public async Task DeleteBookingAsync(string id)
    {
        await _dataAccessManager.DeleteBookingAsync(id);
    }
}