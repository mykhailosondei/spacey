using Amazon.SecurityToken.Model;

namespace ApplicationLogic.HostIdLogic;

public class HostIdStorageProvider : IHostIdSetter, IHostIdGetter
{
    private Guid _hostId;
    
    public Guid HostId
    {
        get => GetHostIdValidation(_hostId);
        set => SetHostIdValidation(value);
    }
    
    private Guid GetHostIdValidation(Guid hostId)
    {
        Console.WriteLine($"Getter: {hostId}, instance: {GetHashCode()}");
        if (hostId == Guid.Empty)
        {
            throw new InvalidIdentityTokenException(hostId.ToString());
        }

        return hostId;
    }
    
    private void SetHostIdValidation(Guid hostId)
    {
        if (hostId == Guid.Empty)
        {
            throw new InvalidIdentityTokenException(hostId.ToString());
        }

        Console.WriteLine($"Setter: {hostId}, instance: {GetHashCode()}");

        _hostId = hostId;
    }
}