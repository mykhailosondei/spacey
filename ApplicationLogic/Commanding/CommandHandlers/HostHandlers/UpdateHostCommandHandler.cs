using ApplicationCommon.DTOs.Host;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.HostCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.HostHandlers;

public class UpdateHostCommandHandler : BaseHandler, IRequestHandler<UpdateHostCommand, Unit>
{
    private readonly IHostCommandAccess _hostCommandAccess;
    
    public UpdateHostCommandHandler(IMapper mapper, IHostCommandAccess hostCommandAccess) : base(mapper)
    {
        _hostCommandAccess = hostCommandAccess;
    }

    public async Task<Unit> Handle(UpdateHostCommand request, CancellationToken cancellationToken)
    {
        var hostDTO = _mapper.Map<HostDTO>(request.Host);
        var host = _mapper.Map<Host>(hostDTO);
        await _hostCommandAccess.UpdateHost(request.Id, host);
        
        return Unit.Value;
    }
}