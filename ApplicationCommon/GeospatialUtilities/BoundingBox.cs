namespace ApplicationCommon.GeospatialUtilities;

public struct BoundingBox
{
    public Coordinates LowerLeft { get; set; }
    public Coordinates UpperRight { get; set; }
    
    public BoundingBox(Coordinates lowerLeft, Coordinates upperRight)
    {
        LowerLeft = lowerLeft;
        UpperRight = upperRight;
    }

    public bool IsValid()
    {
        bool lowerLeftIsValid = LowerLeft.IsValid();
        bool upperRightIsValid = UpperRight.IsValid();
        bool lowerLeftIsSouthWestOfUpperRight = LowerLeft.Latitude <= UpperRight.Latitude && LowerLeft.Longitude <= UpperRight.Longitude;
        return lowerLeftIsValid && upperRightIsValid && lowerLeftIsSouthWestOfUpperRight;
    }
}