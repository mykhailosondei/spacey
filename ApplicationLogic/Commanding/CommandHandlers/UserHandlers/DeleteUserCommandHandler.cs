using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.UserCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using CustomMediator;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ApplicationLogic.Commanding.CommandHandlers.UserHandlers;

public class DeleteUserCommandHandler : BaseHandler, IRequestHandler<DeleteUserCommand>
{
    private readonly IUserCommandAccess _userCommandAccess;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    
    public DeleteUserCommandHandler(IMapper mapper, IUserCommandAccess userCommandAccess, IUserIdGetter userIdGetter, IUserQueryRepository userQueryRepository, IMemoryCache memoryCache, ILogger<DeleteUserCommandHandler> logger) : base(mapper)
    {
        _userCommandAccess = userCommandAccess;
        _userIdGetter = userIdGetter;
        _userQueryRepository = userQueryRepository;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetUserById(request.Id);
        
        if (user == null)
        {
            throw new NotFoundException("User");
        }
        
        if (user.Id != _userIdGetter.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this user.");
        }
        
        await _userCommandAccess.DeleteUser(request.Id);
        
        var cacheKey = $"User_{request.Id.ToString()}";
        _memoryCache.Remove(cacheKey);
        _logger.LogInformation("User: Cache removed");
    }
}