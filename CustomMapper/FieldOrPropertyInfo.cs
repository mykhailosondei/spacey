using System.Reflection;

namespace CustomMapper;

public class FieldOrPropertyInfo
{
    public FieldOrPropertyInfo(FieldInfo fieldInfo)
    {
        Name = fieldInfo.Name;
        Type = fieldInfo.FieldType;
    }
    
    public FieldOrPropertyInfo(PropertyInfo propertyInfo)
    {
        Name = propertyInfo.Name;
        Type = propertyInfo.PropertyType;
    }
        
    public string Name { get; }
    public Type Type { get; }
}