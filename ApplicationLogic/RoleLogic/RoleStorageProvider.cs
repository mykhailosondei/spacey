namespace ApplicationLogic.RoleLogic;

public class RoleStorageProvider : IRoleGetter, IRoleSetter
{
    public IEnumerable<string> Roles { get; set; }

    public bool IsInRole(string role)
    {
        return Roles.Contains(role);
    }
}