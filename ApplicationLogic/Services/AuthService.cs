using System.Globalization;
using ApplicationDataAccess.DataAccess;
using ApplicationDataAccess.Models;
using ApplicationLogic.DTOs;
using ApplicationLogic.Exceptions;

namespace ApplicationLogic.Services;

public class AuthService : BaseService
{
    public AuthService(DataAccessManager dataAccessManager) : base(dataAccessManager)
    {
        
    }

    public async Task Authorize(UserLoginDTO userLoginDto)
    {
        
    }
}