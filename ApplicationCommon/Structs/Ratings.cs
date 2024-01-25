using System.Collections.Immutable;

namespace ApplicationCommon.Structs;

public struct Ratings
{
    private readonly double[] RatingsArray = new double[6];
    
    public double[] getRatingsArray()
    {
        return RatingsArray;
    }

    public Ratings()
    {
        RatingsArray = new double[6];
    }
    
    private static double? TryGetRating(double[] ratingArray, int index)
    {
        if (ratingArray.Length > index)
        {
            return ratingArray[index];
        }

        return null;
    }


    public Ratings(double[] ratingArray)
    {
        Cleanliness = TryGetRating(ratingArray, 0) ?? 0;
        Accuracy = TryGetRating(ratingArray, 1) ?? 0;
        Communication = TryGetRating(ratingArray, 2) ?? 0;
        Location = TryGetRating(ratingArray, 3) ?? 0;
        CheckIn = TryGetRating(ratingArray, 4) ?? 0;
        Value = TryGetRating(ratingArray, 5) ?? 0;
    }
    
    public double Cleanliness
    {
        get { return RatingsArray[0]; }
        set { RatingsArray[0] = value; }
    }


    public double Accuracy
    {
        get { return RatingsArray[1]; }
        set { RatingsArray[1] = value; }
    }

    public double Communication
    {
        get { return RatingsArray[2]; }
        set { RatingsArray[2] = value; }
    }

    public double Location
    {
        get { return RatingsArray[3]; }
        set { RatingsArray[3] = value; }
    }

    public double CheckIn
    {
        get { return RatingsArray[4]; }
        set { RatingsArray[4] = value; }
    }

    public double Value
    {
        get { return RatingsArray[5]; }
        set { RatingsArray[5] = value; }
    }
}