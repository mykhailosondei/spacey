using ApplicationDataAccess.DataAccess;
using ApplicationDataAccess.Models;

namespace ApplicationLogic.Services;

public class UserService : BaseService
{
    public UserService(DataAccessManager dataAccessManager) : base(dataAccessManager)
    {
        
    }
    
    public async Task<List<UserModel>> GetAllUsersAsync()
    { 
        return await _dataAccessManager.GetAllUsersAsync();
    }
    
    public async Task<UserModel> GetUserAsync(string id)
    {
        return await _dataAccessManager.GetUserAsync(id);
    }
 
    public async Task CreateUserAsync(UserModel user)
    {
        await _dataAccessManager.CreateUserAsync(user);
    }
   
    public async Task UpdateUserAsync(string id, UserModel user)
    {
        await _dataAccessManager.UpdateUserAsync(id, user);
    }
    
    public async Task DeleteUserAsync(string id)
    {
        await _dataAccessManager.DeleteUserAsync(id);
    }
}