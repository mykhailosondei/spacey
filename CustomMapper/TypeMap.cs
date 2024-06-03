using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;
using CustomMapper.AssignmentStrategy;
using CustomMapper.AssignmentStrategy.ConcreteStrategies;
using CustomMapper.Static;
using CustomMapper.Structs;

namespace CustomMapper;

public class TypeMap
{
    public TypeMap(MapConfig config)
    {
        Config = config;
    }

    public TypePair TypePair { get; init; }
    public List<PropertyMapInfo> PropertyMapInfos { get; init; }
    public LambdaExpression? MapExpression { get; set; }
    public MapConfig Config { get; set; }

    public void BuildMapExpression()
    {
        if (MapExpression != null)
        {
            return;
        }
        var sourceParam = Parameter(TypePair.Source, "source");
        var destinationParam = Parameter(TypePair.Destination, "destination");
        var body = Block();
        var newAssignment = Assign(destinationParam, New(TypePair.Destination));
        body = body.Append(newAssignment);
        AssignmentContext context = new AssignmentContext();
        foreach (var propInfo in PropertyMapInfos)
        {
            switch (propInfo.Kind)
            {
                case TypeKind.Mappable:
                    context.Strategy = new MappableAssignmentStrategy();
                    break;
                case TypeKind.Custom:
                    var customTypeMap = Config.GetTypeMap(propInfo.CustomPair!.Value);
                    if (customTypeMap == null)
                    {
                        throw new Exception($"Type map not found for types: {propInfo.CustomPair!.Value}");
                    }
                    customTypeMap.BuildMapExpression();
                    context.Strategy = new CustomAssignmentStrategy(customTypeMap.MapExpression!);
                    break;
                case TypeKind.CustomList:
                    var customListTypeMap = Config.GetTypeMap(propInfo.CustomPair!.Value);
                    if (customListTypeMap == null)
                    {
                        throw new Exception($"Type map not found for types: {propInfo.CustomPair!.Value}");
                    }
                    customListTypeMap.BuildMapExpression();
                    context.Strategy = new CustomListAssignmentStrategy(customListTypeMap);
                    break;
            }

            var destinationPropType = propInfo.Kind switch
            {
                TypeKind.CustomList => typeof(List<>).MakeGenericType(propInfo.CustomPair!.Value.Destination),
                _ => propInfo.CustomPair!.Value.Destination
            };
            
            var sourcePropType = propInfo.Kind switch
            {
                TypeKind.CustomList => typeof(List<>).MakeGenericType(propInfo.CustomPair!.Value.Source),
                _ => propInfo.CustomPair!.Value.Source
            };

            Console.WriteLine(sourcePropType);
            Console.WriteLine(destinationPropType);
            
            var assignment = context.BuildAssignment(propInfo, sourceParam, destinationParam);
            var defaultAssignment = Assign(PropertyOrField(destinationParam, propInfo.Name),
                Default(destinationPropType));
            if (ExpressionHelper.IsNullableType(sourcePropType))
            { 
                var nullCheck = IfThenElse(Equal(PropertyOrField(sourceParam, propInfo.Name), Constant(null, sourcePropType)),defaultAssignment,assignment);
                body = body.Append(nullCheck);
            }
            else
            {
                body = body.Append(assignment);
            }
        }
        
        var returnLabel = Label(TypePair.Destination);

        var returnExpression = Label(returnLabel, destinationParam);

        body = body.Append(returnExpression);

        MapExpression = Lambda(body, sourceParam, destinationParam);
    }
}