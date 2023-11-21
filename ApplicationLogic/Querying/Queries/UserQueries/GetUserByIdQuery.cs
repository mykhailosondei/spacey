using ApplicationCommon.DTOs.User;
using MediatR;

namespace ApplicationLogic.Querying.Queries.UserQueries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDTO>;