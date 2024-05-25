using Amazon.Runtime.Internal;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.UserQueries;

public record GetAllUsersQuery() : IRequest<IEnumerable<UserDTO>>;