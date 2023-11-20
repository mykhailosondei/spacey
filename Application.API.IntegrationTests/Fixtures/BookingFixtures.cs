using ApplicationCommon.DTOs.Booking;

namespace Application.API.IntegrationTests.Fixtures;

public static class BookingFixtures
{
    public static BookingCreateDTO BookingCreateDTO = new()
    {
        ListingId = new Guid(),
        CheckIn = DateTime.Now,
        CheckOut = DateTime.Now.AddDays(1)
    };
    
    public static BookingUpdateDTO BookingUpdateDTO = new()
    {
        CheckIn = DateTime.Now.AddDays(1),
        CheckOut = DateTime.Now.AddDays(2)
    };
}