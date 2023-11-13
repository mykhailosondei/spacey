using ApplicationDAL.Entities;

namespace ApplicationDAL.DataCommandAccess;

public interface IUserCommandAccess
{
    public Task<Guid> AddUser(User user);
    public Task UpdateUser(Guid id, User user);
    public Task DeleteUser(Guid id);
}