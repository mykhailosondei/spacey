using ApplicationDataAccess.DataAccess;

namespace ApplicationLogic.Services;

public class UserService : BaseService
{
    public UserService(DataAccessManager dataAccessManager) : base(dataAccessManager)
    {
        
    }
    
    
}