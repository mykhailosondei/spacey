using ApplicationDAL.DataCommandAccess;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.UserCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.UserHandlers;

public class DeleteUserCommandHandler : BaseHandler, IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserCommandAccess _userCommandAccess;
    
    public DeleteUserCommandHandler(IMapper mapper, IUserCommandAccess userCommandAccess) : base(mapper)
    {
        _userCommandAccess = userCommandAccess;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userCommandAccess.DeleteUser(request.Id);
        
        return Unit.Value;
    }
}