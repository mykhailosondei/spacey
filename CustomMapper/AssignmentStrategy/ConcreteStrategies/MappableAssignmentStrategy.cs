using System.Linq.Expressions;
using CustomMapper.AssignmentStrategy.Abstract;
using CustomMapper.Structs;
using static System.Linq.Expressions.Expression;

namespace CustomMapper.AssignmentStrategy.ConcreteStrategies;

public class MappableAssignmentStrategy : IAssignmentStrategy
{
    public BinaryExpression BuildAssignment(PropertyMapInfo propertyMapInfo, ParameterExpression sourceParam, ParameterExpression destinationParam)
    {
        var assignment = Assign(
            PropertyOrField(destinationParam, propertyMapInfo.Name),
            PropertyOrField(sourceParam, propertyMapInfo.Name)
        );
        return assignment;
    }
}