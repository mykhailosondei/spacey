namespace ApplicationCommon.Structs;

public struct Address
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    
    public static bool operator ==(Address a, Address b)
    {
        if (b == null)
        {
            return false;
        }
        return a.Street == b.Street && a.City == b.City && a.Country == b.Country;
    }

    public static bool operator !=(Address a, Address b)
    {
        return !(a == b);
    }
}