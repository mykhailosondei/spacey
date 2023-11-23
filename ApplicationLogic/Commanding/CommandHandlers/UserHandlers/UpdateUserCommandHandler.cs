using ApplicationCommon.DTOs.User;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.UserCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.UserIdLogic;
using User = ApplicationDAL.Entities.User;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.UserHandlers;

public class UpdateUserCommandHandler : BaseHandler, IRequestHandler<UpdateUserCommand>
{
    private readonly  IUserCommandAccess _userCommandAccess;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserIdGetter _userIdGetter;
    
    public UpdateUserCommandHandler(IMapper mapper, IUserCommandAccess userCommandAccess, IUserIdGetter userIdGetter, IUserQueryRepository userQueryRepository) : base(mapper)
    {
        _userCommandAccess = userCommandAccess;
        _userIdGetter = userIdGetter;
        _userQueryRepository = userQueryRepository;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userDTO = _mapper.Map<UserDTO>(request.User);
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
        
        await _userCommandAccess.UpdateUser(request.Id, user);
    }
}