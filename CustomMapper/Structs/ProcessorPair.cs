using CustomMapper.Config;

namespace CustomMapper.Structs;

public record struct ProcessorPair<TSource, TDestination>(
    Action<TSource, TDestination, MapContext> PreProcessor,
    Action<TSource, TDestination, MapContext> PostProcessor);