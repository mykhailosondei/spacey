using System.Linq.Expressions;
using CustomMapper.Structs;
using static System.Linq.Expressions.Expression;

namespace CustomMapper.Static;

public static class ExpressionHelper
{
    public static BlockExpression Append(this BlockExpression block, Expression expression)
    {
        return Block(block.Variables, block.Expressions.ToList().Append(expression));
    }
    
    public static List<PropertyMapInfo> ConstructCommonMapInfos(Type sourceType, Type destinationType)
    {
        var sourceFoPsFromProperties = sourceType.GetProperties().Select(p => new FieldOrPropertyInfo(p));
        var sourceFoPsFromFields = sourceType.GetFields().Select(f => new FieldOrPropertyInfo(f));
        var destinationFoPsFromProperties = destinationType.GetProperties().Select(p => new FieldOrPropertyInfo(p));
        var destinationFoPsFromFields = destinationType.GetFields().Select(f => new FieldOrPropertyInfo(f));

        var sourceProperties = sourceFoPsFromProperties.Concat(sourceFoPsFromFields);
        var destinationProperties = destinationFoPsFromProperties.Concat(destinationFoPsFromFields);

        IEnumerable<(FieldOrPropertyInfo sourceProp, FieldOrPropertyInfo destinationProp)> joined = sourceProperties.GroupJoin(destinationProperties,
            sourceProp => sourceProp.Name,
            destinationProp => destinationProp.Name,
            (sourceProp, destinationProps) => (sourceProp, destinationProps))
            .Where(tup => tup.destinationProps.Any())
            .Select(tup => (tup.sourceProp, tup.destinationProps.First()));

        var result = new List<PropertyMapInfo>();
        foreach (var (sourceProp, destinationProp) in joined)
        {
            if (sourceProp.Type == destinationProp.Type || destinationProp.Type.IsAssignableFrom(sourceProp.Type))
            {
                result.Add(new PropertyMapInfo(sourceProp.Name, TypeKind.Mappable, new TypePair(sourceProp.Type, destinationProp.Type)));
                continue;
            }

            if (AreTypesCustomList(sourceProp.Type, destinationProp.Type))
            {
                result.Add(
                    new PropertyMapInfo(sourceProp.Name,
                        TypeKind.CustomList,
                        new TypePair(GetGenericTypeOfList(sourceProp.Type),
                            GetGenericTypeOfList(destinationProp.Type)))
                    );
                continue;
            }

            if (AreTypesCustom(sourceProp.Type, destinationProp.Type))
            {
                result.Add(
                    new PropertyMapInfo(sourceProp.Name,
                        TypeKind.Custom,
                        new TypePair(sourceProp.Type, destinationProp.Type))
                    );
            }
        }

        return result;
    }

    public static Type GetGenericTypeOfList(Type type)
    {
        return type.GetGenericArguments()[0];
    }

    public static bool IsCoreType(Type type)
    {
        return type.IsPrimitive || type.Assembly == typeof(bool).Assembly;
    }

    public static bool IsCustomList(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) && !IsCoreType(type.GetGenericArguments()[0]);
    }

    public static bool AreTypesCustomList(Type sourceType, Type destinationType)
    {
        return IsCustomList(sourceType) && IsCustomList(destinationType);
    }

    public static bool AreTypesCustom(Type sourceType, Type destinationType)
    {
        return !IsCoreType(sourceType) && !IsCoreType(destinationType);
    }
    
    public static bool IsNullableType(Type type)
    {
        return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
    }
}