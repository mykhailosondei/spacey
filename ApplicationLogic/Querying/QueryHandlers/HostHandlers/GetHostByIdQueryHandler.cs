using ApplicationCommon.DTOs.Host;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Querying.Queries.HostQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.HostHandlers;

public class GetHostByIdQueryHandler : BaseHandler, IRequestHandler<GetHostByIdQuery, HostDTO>
{
    private readonly IHostQueryRepository _hostQueryRepository;
    
    public GetHostByIdQueryHandler(IMapper mapper, IHostQueryRepository hostQueryRepository) : base(mapper)
    {
        _hostQueryRepository = hostQueryRepository;
    }

    public async Task<HostDTO> Handle(GetHostByIdQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<HostDTO>(await _hostQueryRepository.GetHostById(request.Id));
    }
}