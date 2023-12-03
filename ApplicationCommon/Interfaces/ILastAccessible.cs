namespace ApplicationCommon.Interfaces;

public interface ILastAccessible
{
    public Guid Id { get; set; }
    
    public DateTime LastAccess { get; set; }
}