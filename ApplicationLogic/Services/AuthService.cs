using ApplicationCommon.DTOs.User;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Jwt;
using AutoMapper;
using MongoDB.Bson;

namespace ApplicationLogic.Services;

public class AuthService
{

    private readonly IMapper _mapper;
    private readonly IUserCommandAccess _userCommandAccess;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IHostCommandAccess _hostCommandAccess;
    
    private readonly ITokenGenerator _tokenGenerator;

    public AuthService(IMapper mapper, ITokenGenerator tokenGenerator, IUserQueryRepository userQueryRepository, IUserCommandAccess userCommandAccess, IHostCommandAccess hostCommandAccess)
    {
        _mapper = mapper;
        _tokenGenerator = tokenGenerator;
        _userQueryRepository = userQueryRepository;
        _userCommandAccess = userCommandAccess;
        _hostCommandAccess = hostCommandAccess;
    }

    public async Task<AuthUser> SwitchRole(Guid userId, bool toHost)
    {
        var user = await _userQueryRepository.GetUserById(userId);
        
        var token = await _tokenGenerator.GenerateAccessToken(user.Id, user.Host.Id, user.Name, user.Email, isHost: toHost);
        
        var userResultDTO = _mapper.Map<UserDTO>(user);
        
        return new AuthUser {User = userResultDTO, Token = token.Token};
    }

    public async Task<AuthUser> Register(RegisterUserDTO registerUserDTO)
    {
        var userDTO = _mapper.Map<UserDTO>(registerUserDTO);
        var user = _mapper.Map<User>(userDTO);
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password);
        
        Guid userId = await _userCommandAccess.AddUser(user);
        
        Host host = new()
        {
            Id = Guid.NewGuid(),
            UserId = userId
        };
        
        await _hostCommandAccess.AddHost(host);
        user.Host = host;
        await _userCommandAccess.UpdateUser(user.Id, user);
        var userWithId = await _userQueryRepository.GetUserById(userId);
        Console.WriteLine(userWithId.ToBsonDocument());
        var userResultDTO = _mapper.Map<UserDTO>(userWithId);
        var token = await _tokenGenerator.GenerateAccessToken(userWithId.Id, userWithId.Host.Id, userWithId.Name, userWithId.Email, isHost: false);
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

        Console.WriteLine(user.ToBsonDocument());
        
        var token = await _tokenGenerator.GenerateAccessToken(user.Id, user.Host.Id, user.Name, user.Email, isHost: false);
        
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