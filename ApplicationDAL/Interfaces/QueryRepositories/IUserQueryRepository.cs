using ApplicationDAL.Entities;

namespace ApplicationDAL.Interfaces.QueryRepositories;

public interface IUserQueryRepository
{
    public Task<User> GetUserById(Guid id);

    public Task<IEnumerable<User>> GetAllUsers();
    
    public Task<User> GetUserByEmail(string email);
}