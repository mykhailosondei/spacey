namespace ApplicationLogic.Helpers;

public static class BookingHelper
{
    private const double CleaningFeePercentage = 0.1;
    private const double ServiceFeePercentage = 0.05;
    
    public static bool IsDateRangeValid(DateTime checkIn, DateTime checkOut)
    {
        return checkIn < checkOut;
    }
    
    public static bool DateIntersects(DateTime inputCheckIn, DateTime inputCheckOut, (DateTime checkIn, DateTime checkOut)[] existingBookings)
    {
        return existingBookings.Any(existingBooking => inputCheckIn < existingBooking.checkOut && inputCheckOut > existingBooking.checkIn);
    }

    public static int CalculateTotalPrice(DateTime bookingCheckIn, DateTime bookingCheckOut, int listingEntityPricePerNight)
    {
        var days = (bookingCheckOut - bookingCheckIn).Days;
        var totalPrice = days * listingEntityPricePerNight;
        var cleaningFee = (int) (totalPrice * CleaningFeePercentage);
        var serviceFee = (int) (totalPrice * ServiceFeePercentage);
        
        return totalPrice + cleaningFee + serviceFee;
    }

    public static double[] CalculateNewRating(int bookingsIdsCount, double[] listingRatings, double[] reviewRatings)
    {
        if(listingRatings == null || listingRatings.Length == 0)
        {
            return reviewRatings;
        }
        if(listingRatings.Length != reviewRatings.Length)
        {
            throw new ArgumentException("The number of ratings in the review must match the number of ratings in the listing.");
        }
        var newRatings = new double[reviewRatings.Length];
        double [] totalRatings = new double[reviewRatings.Length];
        var totalNumberOfRatings = bookingsIdsCount + 1;
        
        for (var i = 0; i < reviewRatings.Length; i++)
        {
            totalRatings[i] = listingRatings[i] * bookingsIdsCount + reviewRatings[i];
            newRatings[i] = totalRatings[i] / totalNumberOfRatings;
        }

        return newRatings;
    }
}