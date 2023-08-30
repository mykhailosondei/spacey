namespace ApplicationLogic.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name) : base($"Entity {name} was not found")
    {
        
    }
}