namespace CustomMapper.Structs;

public record struct TypePair(Type Source, Type Destination)
{
    public TypePair Reverse() => new (Destination, Source);
    public override string ToString()
    {
        return $"{Source.FullName} -> {Destination.FullName}";
    }
}