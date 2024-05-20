using ApplicationCommon.DTOs.Host;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.HostQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

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
        var result = await _hostQueryRepository.GetHostById(request.Id);
        
        if (result == null)
        {
            throw new NotFoundException(nameof(HostDTO));
        }
        
        result.LastAccess = DateTime.UtcNow;
        
        var mappedHost = _mapper.Map<HostDTO>(result);
        
        return mappedHost;
    }
}