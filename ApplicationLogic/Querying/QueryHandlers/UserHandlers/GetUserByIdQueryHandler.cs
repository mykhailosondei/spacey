using ApplicationCommon.DTOs.User;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
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
        var result = await _userQueryRepository.GetUserById(request.Id);
        
        if (result == null)
        {
            throw new NotFoundException(nameof(UserDTO));
        }
        
        return _mapper.Map<UserDTO>(result);
    }
}