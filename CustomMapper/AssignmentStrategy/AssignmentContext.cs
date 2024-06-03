using System.Linq.Expressions;
using CustomMapper.AssignmentStrategy.Abstract;
using CustomMapper.Structs;

namespace CustomMapper.AssignmentStrategy;

public class AssignmentContext
{
    public IAssignmentStrategy? Strategy { private get; set; }

    public BinaryExpression BuildAssignment(PropertyMapInfo propertyMapInfo, ParameterExpression sourceParam, ParameterExpression destinationParam)
    {
        if (Strategy == null)
        {
            throw new ArgumentException("Strategy is not specified");
        }
        return Strategy.BuildAssignment(propertyMapInfo, sourceParam, destinationParam);
    }
}