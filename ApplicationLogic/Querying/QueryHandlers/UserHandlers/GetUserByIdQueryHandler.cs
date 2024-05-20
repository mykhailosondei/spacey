using ApplicationCommon.DTOs.User;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.UserQueries;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace ApplicationLogic.Querying.QueryHandlers.UserHandlers;

public class GetUserByIdQueryHandler : BaseHandler, IRequestHandler<GetUserByIdQuery, UserDTO>
{
    private readonly IUserQueryRepository _userQueryRepository;
    
    public GetUserByIdQueryHandler(IMapper mapper, IUserQueryRepository userQueryRepository) : base(mapper)
    {
        _userQueryRepository = userQueryRepository;
    }

    public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _userQueryRepository.GetUserById(request.Id);

        if (result == null)
        {
            throw new NotFoundException(nameof(UserDTO));
        }

        result.LastAccess = DateTime.UtcNow;

        var mappedUser = _mapper.Map<UserDTO>(result);

        Console.WriteLine(mappedUser.ToBsonDocument(configurator: builder =>
        {
            
        }).ToJson());
        
        return mappedUser;
    }
}