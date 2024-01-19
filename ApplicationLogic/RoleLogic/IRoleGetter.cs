namespace ApplicationLogic.RoleLogic;

public interface IRoleGetter
{
    IEnumerable<string> Roles { get; }
    
    public bool IsInRole(string role);
}