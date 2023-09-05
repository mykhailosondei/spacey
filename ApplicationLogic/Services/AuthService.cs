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
        var userEntities = await _dataAccessManager.GetAllUsersAsync();
        var userEntity = userEntities.FirstOrDefault(u => u.Email == userLoginDto.Email);

        if (userEntity == null)
        {
            throw new NotFoundException(nameof(UserModel));
        }

        if (userEntity.Password != userLoginDto.Password)
        {
            throw new Exception("wrong password");
        }
    }
}