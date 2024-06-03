namespace CustomMapper.Config;

public class MapContext
{
    public Dictionary<string, object> Items { get; set; } = new ();
    public static readonly MapContext DefaultContext = new ();
}