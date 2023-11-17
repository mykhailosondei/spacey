using ApplicationCommon.DTOs.User;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Querying.Queries.UserQueries;
using AutoMapper;
using MediatR;

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
        return _mapper.Map<UserDTO>(await _userQueryRepository.GetUserById(request.id));
    }
}