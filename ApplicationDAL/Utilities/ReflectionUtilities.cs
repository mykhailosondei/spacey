using System.Reflection;
using MongoDB.Bson;

namespace ApplicationDAL.Utilities;

public static class ReflectionUtilities
{
    public static IEnumerable<KeyValuePair<string, object?>> GetPropertiesThatAreNotMarkedWithAttribute<TSource,TAttribute>(TSource instance)
        where TAttribute : Attribute
    {
        var type = instance.GetType();
        return type.GetProperties()
            .Where(p => p.GetCustomAttribute<TAttribute>() == null)
            .Select(p => new KeyValuePair<string, object?>(p.Name, p.GetValue(instance)));
    }

    public static IEnumerable<string> GetPropertyNamesThatAreMarkedWithAttribute<TAttribute>(Type type) where TAttribute : Attribute
    {
        return type.GetProperties().Where(p => p.GetCustomAttribute<TAttribute>() != null).Select(p => p.Name);
    }
}