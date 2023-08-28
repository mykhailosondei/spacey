using ApplicationDataAccess.DataAccess;

namespace ApplicationLogic.Services;

public abstract class BaseService
{
    private protected readonly DataAccessManager _dataAccessManager;

    protected BaseService(DataAccessManager dataAccessManager)
    {
        _dataAccessManager = dataAccessManager;
    }
}