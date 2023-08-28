using ApplicationDataAccess.DataAccess;
using ApplicationDataAccess.Models;

namespace ApplicationLogic.Services;

public class HostService : BaseService
{
    public HostService(DataAccessManager dataAccessManager) : base(dataAccessManager)
    {
        
    }
    
    public async Task<List<HostModel>> GetAllHosts()
    {
        return await _dataAccessManager.GetAllHosts();
    }

    
    public async Task<HostModel> GetHost(string id)
    {
        return await _dataAccessManager.GetHost(id);
    }

    
    public async Task CreateHostAsync( HostModel model)
    {
        await _dataAccessManager.CreateHostAsync(model);
    }

    
    public async Task UpdateHostAsync(string id,  HostModel model)
    {
        await _dataAccessManager.UpdateHostAsync(id, model);
    }

    
    public async Task DeleteHostAsync(string id)
    {
        await _dataAccessManager.DeleteHostAsync(id);
    }
}