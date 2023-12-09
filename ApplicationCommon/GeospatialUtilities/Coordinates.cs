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
}