using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.HostCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.HostHandlers;

public class DeleteHostCommandHandler :BaseHandler, IRequestHandler<DeleteHostCommand>
{
    private readonly IHostCommandAccess _hostCommandAccess;
    
    public DeleteHostCommandHandler(IMapper mapper, IHostCommandAccess hostCommandAccess) : base(mapper)
    {
        _hostCommandAccess = hostCommandAccess;
    }

    public Task Handle(DeleteHostCommand request, CancellationToken cancellationToken)
    {
        return _hostCommandAccess.DeleteHost(request.Id);
    }
}