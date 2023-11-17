using ApplicationCommon.DTOs.User;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.UserCommands;
using User = ApplicationDAL.Entities.User;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.UserHandlers;

public class UpdateUserCommandHandler : BaseHandler, IRequestHandler<UpdateUserCommand>
{
    private readonly  IUserCommandAccess _userCommandAccess;
    
    public UpdateUserCommandHandler(IMapper mapper, IUserCommandAccess userCommandAccess) : base(mapper)
    {
        _userCommandAccess = userCommandAccess;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userDTO = _mapper.Map<UserDTO>(request.User);
        var user = _mapper.Map<User>(userDTO);
        await _userCommandAccess.UpdateUser(request.Id, user);
    }
}