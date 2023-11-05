using System.Reflection;

namespace ApplicationLogic;

public static class AssemblyMarker
{
    public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
}