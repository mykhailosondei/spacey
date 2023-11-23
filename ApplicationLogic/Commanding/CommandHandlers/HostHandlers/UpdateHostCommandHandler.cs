using ApplicationCommon.DTOs.Host;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.HostCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.HostHandlers;

public class UpdateHostCommandHandler : BaseHandler, IRequestHandler<UpdateHostCommand>
{
    private readonly IHostCommandAccess _hostCommandAccess;
    private readonly IHostQueryRepository _hostQueryRepository;
    private readonly IHostIdGetter _hostIdGetter;
    
    public UpdateHostCommandHandler(IMapper mapper, IHostCommandAccess hostCommandAccess, IHostQueryRepository hostQueryRepository, IHostIdGetter hostIdGetter) : base(mapper)
    {
        _hostCommandAccess = hostCommandAccess;
        _hostQueryRepository = hostQueryRepository;
        _hostIdGetter = hostIdGetter;
    }

    public async Task Handle(UpdateHostCommand request, CancellationToken cancellationToken)
    {
        var hostDTO = _mapper.Map<HostDTO>(request.Host);
        var host = _mapper.Map<Host>(hostDTO);
        
        var existingHost = await _hostQueryRepository.GetHostById(request.Id);
        
        if (existingHost == null)
        {
            throw new NotFoundException("Host");
        }
        
        if(existingHost.Id != _hostIdGetter.HostId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this host.");
        }
        
        
        await _hostCommandAccess.UpdateHost(request.Id, host);
    }
}