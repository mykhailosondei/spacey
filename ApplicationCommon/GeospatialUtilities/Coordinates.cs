namespace ApplicationCommon.GeospatialUtilities;

public struct Coordinates
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public Coordinates(double longitude, double latitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
    
    public bool IsValid()
    {
        return Latitude is >= -90 and <= 90 && Longitude is >= -180 and <= 180;
    }

    public static double GetDistance(Coordinates requestCoordinates, Coordinates listingCoordinates)
    {
        if (!requestCoordinates.IsValid())
        {
            throw new ArgumentException("Invalid request coordinates");
        }
        
        if (!listingCoordinates.IsValid())
        {
            throw new ArgumentException("Invalid listing coordinates");
        }
        
        const double earthRadius = 6371;
        
        var dLatitude = ToRadians(listingCoordinates.Latitude - requestCoordinates.Latitude);
        var dLongitude = ToRadians(listingCoordinates.Longitude - requestCoordinates.Longitude);
        
        var a = Math.Sin(dLatitude / 2) * Math.Sin(dLatitude / 2) +
                Math.Cos(ToRadians(requestCoordinates.Latitude)) * Math.Cos(ToRadians(listingCoordinates.Latitude)) *
                Math.Sin(dLongitude / 2) * Math.Sin(dLongitude / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return earthRadius * c;
    }
    
    private static double ToRadians(double val)
    {
        return Math.PI * val / 180;
    }
}