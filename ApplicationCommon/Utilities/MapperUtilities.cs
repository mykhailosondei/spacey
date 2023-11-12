using ApplicationCommon.Enums;

namespace ApplicationCommon.Utilities;

public static class MapperUtilities
{
    public static Amenities ConstructAmenitiesFromStringArray(string[] amenities)
    {
        return (Amenities)amenities.Aggregate(0, (current, amenity) =>
        {
            if (Enum.TryParse(typeof(Amenities), amenity, out object? parsedAmenity))
            {
                return current | (int)Enum.Parse(typeof(Amenities), amenity);
            }
            throw new ArgumentException($"Amenity {amenity} is not valid.");
        });
    }

    public static string[] ConstructStringArrayFromAmenities(Amenities amenities)
    {
        var result = new List<string>();
        
        var bitValue = Convert.ToString((int)amenities, 2);
        
        for (var i = 0; i < bitValue.Length; i++)
        {
            if (bitValue[i] == '1')
            {
                result.Add(Enum.GetName(typeof(Amenities), (int)Math.Pow(2, bitValue.Length - i - 1))!);
            }
        }
        
        return result.ToArray();
    }
}