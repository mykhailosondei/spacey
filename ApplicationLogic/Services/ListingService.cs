using ApplicationDataAccess.DataAccess;
using ApplicationDataAccess.Models;

namespace ApplicationLogic.Services;

public class ListingService : BaseService
{
    public ListingService(DataAccessManager dataAccessManager) : base(dataAccessManager)
    {
        
    }
    
    public async Task<List<ListingModel>> GetAllListingsAsync()
    {
        return await _dataAccessManager.GetAllListingsAsync();
    }

    
    public async Task<ListingModel> GetListingAsync(string id)
    {
        return await _dataAccessManager.GetListingAsync(id);
    }
    
    public async Task CreateListingAsync( ListingModel value)
    {
        await _dataAccessManager.CreateListingAsync(value);
    }

    
    public async Task UpdateListingAsync(string id, ListingModel model)
    {
        await _dataAccessManager.UpdateListingAsync(id, model);
    }

    
    public async Task DeleteListingAsync(string id)
    {
        await _dataAccessManager.DeleteListingAsync(id);
    }
}