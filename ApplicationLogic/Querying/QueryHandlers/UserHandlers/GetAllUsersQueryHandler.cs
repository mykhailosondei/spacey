using Amazon.Runtime.Internal;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.UserQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.User;

public class GetAllUsersQueryHandler : BaseHandler, IRequestHandler<GetAllUsersQuery, IEnumerable<UserDTO>>
{
    private readonly IUserQueryRepository _userQueryRepository;
    
    public GetAllUsersQueryHandler(IUserQueryRepository userQueryRepository, IMapper mapper) : base(mapper)
    {
        _userQueryRepository = userQueryRepository;
    }
    
    public async Task<IEnumerable<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _userQueryRepository.GetAllUsers();

        if (!result.Any())
        {
            throw new NotFoundException(nameof(IEnumerable<UserDTO>));
        }
        
        return _mapper.Map<IEnumerable<UserDTO>>(result);
    }
}