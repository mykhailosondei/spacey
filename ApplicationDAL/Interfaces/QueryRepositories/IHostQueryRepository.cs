using ApplicationDAL.Entities;

namespace ApplicationDAL.Interfaces.QueryRepositories;

public interface IHostQueryRepository
{
    public Task<Host> GetHostById(Guid id);
}