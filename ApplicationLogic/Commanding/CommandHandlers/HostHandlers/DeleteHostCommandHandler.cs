using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.HostCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.HostHandlers;

public class DeleteHostCommandHandler :BaseHandler, IRequestHandler<DeleteHostCommand>
{
    private readonly IHostCommandAccess _hostCommandAccess;
    private readonly IHostQueryRepository _hostQueryRepository;
    private readonly IHostIdGetter _hostIdGetter;
    
    public DeleteHostCommandHandler(IMapper mapper, IHostCommandAccess hostCommandAccess, IHostQueryRepository hostQueryRepository, IHostIdGetter hostIdGetter) : base(mapper)
    {
        _hostCommandAccess = hostCommandAccess;
        _hostQueryRepository = hostQueryRepository;
        _hostIdGetter = hostIdGetter;
    }

    public Task Handle(DeleteHostCommand request, CancellationToken cancellationToken)
    {
        var host = _hostQueryRepository.GetHostById(request.Id).Result;
        
        if (host == null)
        {
            throw new NotFoundException("Host");
        }
        
        if(host.Id != _hostIdGetter.HostId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this host.");
        }
        
        return _hostCommandAccess.DeleteHost(request.Id);
    }
}