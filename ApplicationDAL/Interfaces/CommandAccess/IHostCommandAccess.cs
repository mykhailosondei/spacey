using ApplicationDAL.Entities;
using MongoDB.Driver;

namespace ApplicationDAL.Interfaces.CommandAccess;

public interface IHostCommandAccess
{
    public Task<Guid> AddHost(Host host);

    public Task UpdateHost(Guid id, Host host);

    public Task DeleteHost(Guid id);
}