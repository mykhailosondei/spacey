using CustomMapper;
using CustomMapper.Config;
using CustomMapper.Static;
using CustomMapper.Structs;

public class MapConfig
{
    public List<TypeMap> TypeMaps { get; } = new();
    public Dictionary<TypePair, object> OptionsDictionary = new();


    public TypeMapBuilder<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
        var typeMap = new TypeMap(Instance)
        {
            TypePair = new TypePair(typeof(TSource), typeof(TDestination)),
            PropertyMapInfos = ExpressionHelper.ConstructCommonMapInfos(typeof(TSource), typeof(TDestination))
        };
        TypeMaps.Add(typeMap);
        return new TypeMapBuilder<TSource, TDestination>(Instance);
    }

    public void BuildMapExpression<TSource, TDestination>()
    {
        var typeMap = GetTypeMap<TSource, TDestination>();
        if (typeMap == null)
        {
            throw new Exception("Type map not found");
        }

        if (typeMap.MapExpression != null)
        {
            return;
        }

        typeMap.BuildMapExpression();
    }

    public TypeMap? GetTypeMap<TSource, TDestination>()
    {
        return TypeMaps.FirstOrDefault(tm => tm.TypePair.Source == typeof(TSource) && tm.TypePair.Destination == typeof(TDestination));
    }

    public TypeMap? GetTypeMap(TypePair typePair)
    {
        return TypeMaps.FirstOrDefault(tm => tm.TypePair == typePair);
    }

    public void RegisterMap<TSource, TDestination>(ProcessorPair<TSource, TDestination> processorPair, Dictionary<NameType, object> memberMapFunctions)
    {
        var typePair = new TypePair(typeof(TSource), typeof(TDestination));
        var mapOptions = OptionsDictionary.GetValueOrDefault(typePair);
        if (mapOptions == null)
        {
            OptionsDictionary[typePair] = new MapOptions<TSource, TDestination>()
            {
                Context = new MapContext(),
                ProcessorPair = processorPair,
                MemberMapFunctions = memberMapFunctions
            };
        }
        else
        {
            var mapOptionsCasted = (MapOptions<TSource, TDestination>)mapOptions;
            mapOptionsCasted.ProcessorPair = processorPair;
            mapOptionsCasted.MemberMapFunctions = memberMapFunctions;
            OptionsDictionary[typePair] = mapOptionsCasted;
        }
    }

    private MapConfig()
    {

    }
    private static MapConfig? _instance;
    public static MapConfig Instance { get; } = _instance ??= new MapConfig();
}
