using ApplicationCommon.DTOs.User;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Jwt;
using AutoMapper;

namespace ApplicationLogic.Services;

public class AuthService
{

    private readonly IMapper _mapper;
    private readonly IUserCommandAccess _userCommandAccess;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthService(IMapper mapper, ITokenGenerator tokenGenerator, IUserQueryRepository userQueryRepository, IUserCommandAccess userCommandAccess)
    {
        _mapper = mapper;
        _tokenGenerator = tokenGenerator;
        _userQueryRepository = userQueryRepository;
        _userCommandAccess = userCommandAccess;
    }

    public async Task<AuthUser> Register(RegisterUserDTO registerUserDTO)
    {
        var userDTO = _mapper.Map<UserDTO>(registerUserDTO);
        var user = _mapper.Map<User>(userDTO);
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password);
        
        Guid userId = await _userCommandAccess.AddUser(user);
        var userWithId = await _userQueryRepository.GetUserById(userId);
        var userResultDTO = _mapper.Map<UserDTO>(userWithId);
        var token = await _tokenGenerator.GenerateAccessToken(userWithId.Id, userWithId.Name, userWithId.Email);
        return new AuthUser {User = userResultDTO, Token = token.Token};
    }

    public async Task<AuthUser> Authorize(LoginUserDTO loginUserDTO)
    {
        var user = await _userQueryRepository.GetUserByEmail(loginUserDTO.Email);
        
        if (user == null)
        {
            throw new NotFoundException(nameof(User));
        }
        
        if (!BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.PasswordHash))
        {
            throw new InvalidPasswordException();
        }
        
        var token = await _tokenGenerator.GenerateAccessToken(user.Id, user.Name, user.Email);
        
        var userResultDTO = _mapper.Map<UserDTO>(user);
        
        return new AuthUser {User = userResultDTO, Token = token.Token};
    }
}

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base("Invalid password")
    {
    }
}