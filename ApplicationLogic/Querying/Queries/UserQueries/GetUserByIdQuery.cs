using ApplicationCommon.DTOs.User;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.UserQueries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDTO>;