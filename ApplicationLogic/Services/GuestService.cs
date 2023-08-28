using ApplicationDataAccess.DataAccess;
using ApplicationDataAccess.Models;

namespace ApplicationLogic.Services;

public class GuestService : BaseService
{
    public GuestService(DataAccessManager dataAccessManager) : base(dataAccessManager)
    {
        
    }
    
    public async Task<List<GuestModel>> GetAllGuestsAsync()
    { 
        return await _dataAccessManager.GetAllGuestsAsync();
    }
    
    public async Task<GuestModel> GetGuestAsync(string id)
    {
        return await _dataAccessManager.GetGuestAsync(id);
    }
 
    public async Task CreateGuestAsync(GuestModel guest)
    {
        await _dataAccessManager.CreateGuestAsync(guest);
    }
   
    public async Task UpdateGuestAsync(string id, GuestModel guest)
    {
        await _dataAccessManager.UpdateGuestAsync(id, guest);
    }
    
    public async Task DeleteGuestAsync(string id)
    {
        await _dataAccessManager.DeleteGuestAsync(id);
    }
}