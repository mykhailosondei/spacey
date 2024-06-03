using System.Reflection;

namespace CustomMapper.Structs;

public struct NameType
{
    public readonly string Name;
    public readonly Type Type;

    public NameType(Type type, string name)
    {
        Type = type;
        Name = name;
    }

    public NameType(MemberInfo member)
    {
        Name = member.Name;
        Type = member.DeclaringType!;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode() & Type.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is NameType ? ((NameType)obj).Equals(this) : false;
    }

    private bool Equals(NameType other)
    {
        return other.Name == Name && other.Type == Type;
    }
}