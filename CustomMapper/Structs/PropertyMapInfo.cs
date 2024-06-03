namespace CustomMapper.Structs;

public record struct PropertyMapInfo(string Name, TypeKind Kind, TypePair? CustomPair)
{
    public override string ToString()
    {
        return $"Name: {Name}, Kind: {Kind}, CustomPair: {(CustomPair == null ? "none" : CustomPair.ToString())}";
    }
};

public enum TypeKind
{
    Mappable,
    Custom,
    CustomList
}