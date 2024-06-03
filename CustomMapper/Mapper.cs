using System.Diagnostics;
using CustomMapper.Config;
using TypePair = CustomMapper.Structs.TypePair;

namespace CustomMapper;

public class Mapper : IMapper
{
    private readonly MapConfig _config;

    public Mapper(MapConfig config)
    {
        _config = config;
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return MapCore(source, default(TDestination), MapContext.DefaultContext);
    }

    public TDestination Map<TSource, TDestination>(TSource source, MapContext context)
    {
        return MapCore(source, default(TDestination), context);
    }
    
    private TDestination MapCore<TSource, TDestination>(TSource source, TDestination destination, MapContext context)
    {
        var typePair = new TypePair(typeof(TSource), typeof(TDestination));
        var typeMap = _config.GetTypeMap(typePair);
        if (typeMap == null)
        {
            throw new ArgumentException($"Map is not defined for this interaction: {typePair}");
        }

        var mapOptions = (MapOptions<TSource, TDestination>)_config.OptionsDictionary[typePair];

        var processors = mapOptions.ProcessorPair;
        processors.PreProcessor.Invoke(source, destination, context);
        destination = (TDestination) typeMap.MapExpression!.Compile().DynamicInvoke(source, destination)!;
        processors.PostProcessor.Invoke(source, destination, context);
        
        foreach (var (propertyInfo, mapFunctionObject) in mapOptions.MemberMapFunctions)
        {
           typeof(TDestination).GetProperty(propertyInfo.Name)!.SetValue(destination, ((Func<TSource,MapContext,object?>)mapFunctionObject).Invoke(source, context));
        }

        return destination;
    }
}

public interface IMapper
{
    public TDestination Map<TSource, TDestination>(TSource source);
    public TDestination Map<TSource, TDestination>(TSource source, MapContext context);
}