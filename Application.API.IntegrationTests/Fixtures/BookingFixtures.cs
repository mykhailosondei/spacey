
using ApplicationCommon.DTOs.BookingDTOs;

namespace Application.API.IntegrationTests.Fixtures;

public static class BookingFixtures
{
    public static BookingCreateDTO BookingCreateDTO = new()
    {
        ListingId = new Guid(),
        CheckIn = DateTime.Now,
        CheckOut = DateTime.Now.AddDays(1),
        NumberOfGuests = 1
    };
    
    public static BookingUpdateDTO BookingUpdateDTO = new()
    {
        CheckIn = DateTime.Now.AddDays(2),
        CheckOut = DateTime.Now.AddDays(3),
        NumberOfGuests = 1
    };
}