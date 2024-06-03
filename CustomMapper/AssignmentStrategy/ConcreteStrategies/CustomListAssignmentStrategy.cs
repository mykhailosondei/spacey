using System.Linq.Expressions;
using CustomMapper.AssignmentStrategy.Abstract;
using CustomMapper.Structs;
using static System.Linq.Expressions.Expression;

namespace CustomMapper.AssignmentStrategy.ConcreteStrategies;

public class CustomListAssignmentStrategy : IAssignmentStrategy
{
    private readonly TypeMap _typeMap;

    public CustomListAssignmentStrategy(TypeMap typeMap)
    {
        _typeMap = typeMap;
    }

    private static LambdaExpression ListMapLambda(TypeMap typeMap)
    {
        var sourceListParameter = Parameter(typeof(List<>).MakeGenericType(typeMap.TypePair.Source), "sourceList");
        var destinationListParameter = Parameter(typeof(List<>).MakeGenericType(typeMap.TypePair.Destination), "destinationList");
        var label = Label(typeof(List<>).MakeGenericType(typeMap.TypePair.Destination));
        var destinationParam = Parameter(typeMap.TypePair.Destination, "destinationParam");


        return Lambda(
            Block(
                Assign(destinationListParameter, New(destinationListParameter.Type)),
                Loop(IfThenElse(GreaterThan(Property(sourceListParameter, "Count"), Constant(0)),
                        Block(
                            new [] {destinationParam},
                            Assign(destinationParam, New(destinationParam.Type)),
                            Assign(destinationParam,
                            Invoke(typeMap.MapExpression!, ArrayIndex(Call(sourceListParameter, "ToArray", null), Constant(0)), destinationParam)
                                ),
                            Call(destinationListParameter, "Add", null, destinationParam),
                            Call(sourceListParameter, "RemoveAt", null, Constant(0))
                        ),
                        Break(label, destinationListParameter)),
                    label)
            )
            ,
            sourceListParameter, destinationListParameter);
    }
    
    public BinaryExpression BuildAssignment(PropertyMapInfo propertyMapInfo, ParameterExpression sourceParam,
        ParameterExpression destinationParam)
    {
        var listMapLambda = ListMapLambda(_typeMap);
        return Assign(
            PropertyOrField(destinationParam, propertyMapInfo.Name),
            Invoke(listMapLambda, PropertyOrField(sourceParam, propertyMapInfo.Name), PropertyOrField(destinationParam, propertyMapInfo.Name))
        );
    }
}