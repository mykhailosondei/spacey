using ApplicationCommon.DTOs.User;

namespace ApplicationLogic.Services;

public interface IRegistrar
{
    public Task<AuthUser> Register(RegisterUserDTO registerUserDTO);
}