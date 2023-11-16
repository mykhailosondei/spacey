using ApplicationCommon.DTOs.Host;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.HostCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.HostHandlers;

public class CreateHostCommandHandler : BaseHandler, IRequestHandler<CreateHostCommand, Guid>
{
    private readonly IHostCommandAccess _hostCommandAccess;
    
    public CreateHostCommandHandler(IMapper mapper, IHostCommandAccess hostCommandAccess) : base(mapper)
    {
        _hostCommandAccess = hostCommandAccess;
    }

    public async Task<Guid> Handle(CreateHostCommand request, CancellationToken cancellationToken)
    {
        var hostDTO = _mapper.Map<HostDTO>(request.Host);
        var host = _mapper.Map<Host>(hostDTO);
        
        var id = await _hostCommandAccess.AddHost(host);
        
        return id;
    }
}