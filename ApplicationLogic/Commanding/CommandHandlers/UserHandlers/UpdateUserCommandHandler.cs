using ApplicationCommon.DTOs.User;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.UserCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Options;
using ApplicationLogic.UserIdLogic;
using User = ApplicationDAL.Entities.User;
using AutoMapper;
using CustomMediator;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApplicationLogic.Commanding.CommandHandlers.UserHandlers;

public class UpdateUserCommandHandler : BaseHandler, IRequestHandler<UpdateUserCommand>
{
    private readonly  IUserCommandAccess _userCommandAccess;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly BingMapsConnectionOptions _bingMapsConnectionOptions;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IMapper mapper, IUserCommandAccess userCommandAccess, IUserIdGetter userIdGetter, IUserQueryRepository userQueryRepository, IOptions<BingMapsConnectionOptions> bingMapsConnectionOptions, IMemoryCache memoryCache, ILogger<UpdateUserCommandHandler> logger) : base(mapper)
    {
        _userCommandAccess = userCommandAccess;
        _userIdGetter = userIdGetter;
        _userQueryRepository = userQueryRepository;
        _memoryCache = memoryCache;
        _logger = logger;
        _bingMapsConnectionOptions = bingMapsConnectionOptions.Value;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userDTO = _mapper.Map<UserDTO>(request.User, options =>
        {
            options.Items["BingMapsKey"] = _bingMapsConnectionOptions.BingMapsKey;
        });
        var user = _mapper.Map<User>(userDTO);
        
        var existingUser = await _userQueryRepository.GetUserById(request.Id);
        
        if (existingUser == null)
        {
            throw new NotFoundException("User");
        }
        
        if (existingUser.Id != _userIdGetter.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this user.");
        }
        
        user.LikedListingsIds = existingUser.LikedListingsIds;
        
        await _userCommandAccess.UpdateUser(request.Id, user);
        
        var cacheKey = $"User_{request.Id.ToString()}";
        _memoryCache.Remove(cacheKey);
        _logger.LogInformation("User: Cache removed");
    }
}