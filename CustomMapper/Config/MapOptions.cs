using CustomMapper.Config;
using CustomMapper.Structs;

public class MapOptions<TSource, TDestination>
{
    public MapContext Context { get; set; }
    public ProcessorPair<TSource, TDestination> ProcessorPair { get; set; }
    public Dictionary<NameType, object> MemberMapFunctions { get; set; } = new ();
}