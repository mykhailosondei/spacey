using System.Linq.Expressions;
using CustomMapper.Structs;

namespace CustomMapper.AssignmentStrategy.Abstract;

public interface IAssignmentStrategy
{
    public BinaryExpression BuildAssignment(PropertyMapInfo propertyMapInfo, ParameterExpression sourceParam, ParameterExpression destinationParam);
}