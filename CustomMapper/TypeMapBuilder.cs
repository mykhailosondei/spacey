using System.Linq.Expressions;
using CustomMapper.Config;
using CustomMapper.Static;
using CustomMapper.Structs;

namespace CustomMapper;

public class TypeMapBuilder<TSource, TDestination>
{
    private readonly MapConfig _config;

    public TypeMapBuilder(MapConfig config)
    {
        _config = config;
    }

    private ProcessorPair<TSource, TDestination> _processorPair = new ();
    private readonly Dictionary<NameType, object> _memberMapFunctions = new ();
    
    public TypeMapBuilder<TSource, TDestination> ReverseMap()
    {
        var typePair = new TypePair(typeof(TDestination), typeof(TSource));
        var typeMap = new TypeMap(_config)
        {
            TypePair = typePair,
            PropertyMapInfos = ExpressionHelper.ConstructCommonMapInfos(typePair.Source, typePair.Destination)
        };
        _config.TypeMaps.Add(typeMap);
        return this;
    }

    public TypeMapBuilder<TSource, TDestination> AfterMap(Action<TSource, TDestination, MapContext> postProcessor)
    {
        _processorPair.PostProcessor = postProcessor;
        return this;
    }
    
    public TypeMapBuilder<TSource, TDestination> BeforeMap(Action<TSource, TDestination, MapContext> preProcessor)
    {
        _processorPair.PreProcessor = preProcessor;
        return this;
    }

    public TypeMapBuilder<TSource, TDestination> ForMember<TMember>(
        Expression<Func<TDestination, TMember>> memberLambda, Func<TSource, MapContext, TMember> mapFunc)
    {
        var memberExpression = memberLambda.Body;
        var memberInfo = memberExpression switch
        {
            MemberExpression { Member: var member, Expression.NodeType: ExpressionType.Parameter } => new NameType(member),
            _ => throw new Exception("Expression should define a member")
        };

        _memberMapFunctions[memberInfo] = mapFunc;
        return this;
    }

    public void Register()
    {
        _config.RegisterMap(_processorPair, _memberMapFunctions);
    }
}