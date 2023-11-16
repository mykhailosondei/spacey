using System.Reflection;

namespace ApplicationLogic;

public class AssemblyMarker
{
    public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
}