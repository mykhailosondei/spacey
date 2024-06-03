using System.Linq.Expressions;
using CustomMapper.AssignmentStrategy.Abstract;
using CustomMapper.Structs;
using static System.Linq.Expressions.Expression;

namespace CustomMapper.AssignmentStrategy.ConcreteStrategies;

public class CustomAssignmentStrategy : IAssignmentStrategy
{
    private readonly LambdaExpression _mapExpression;

    public CustomAssignmentStrategy(LambdaExpression mapExpression)
    {
        _mapExpression = mapExpression;
    }

    public BinaryExpression BuildAssignment(PropertyMapInfo propertyMapInfo, ParameterExpression sourceParam, ParameterExpression destinationParam)
    {
        return Assign(
            PropertyOrField(destinationParam, propertyMapInfo.Name),
            Invoke(_mapExpression,  PropertyOrField(sourceParam, propertyMapInfo.Name), PropertyOrField(destinationParam, propertyMapInfo.Name))
        );
    }
}