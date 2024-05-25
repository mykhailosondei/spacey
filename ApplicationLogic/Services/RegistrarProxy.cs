using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;

namespace ApplicationLogic.Services;

public class RegistrarProxy : IRegistrar
{
    private readonly AuthService _registrar;
    private readonly IUserQueryRepository _userQueryRepository;
    

    public RegistrarProxy(AuthService registrar, IUserQueryRepository userQueryRepository)
    {
        _registrar = registrar;
        _userQueryRepository = userQueryRepository;
    }
    
    private async Task<bool> UserExists(string email)
    {
        User? user = await _userQueryRepository.GetUserByEmail(email);
        return user != null;
    }

    public async Task<AuthUser> Register(RegisterUserDTO registerUserDTO)
    {
        if (await UserExists(registerUserDTO.Email))
        {
            throw new EmailTakenException();
        }
        return await _registrar.Register(registerUserDTO);
    }
}