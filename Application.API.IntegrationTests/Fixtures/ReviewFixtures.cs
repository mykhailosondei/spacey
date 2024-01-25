using ApplicationCommon.DTOs.Review;
using ApplicationCommon.Structs;

namespace Application.API.IntegrationTests.Fixtures;

public static class ReviewFixtures
{
    public static ReviewCreateDTO ReviewCreateDTO = new()
    {
        BookingId = new Guid(),
        Comment = "Test",
        Ratings = new Ratings(new []{1.0,2.0,3.0,4.0,5.0,5.0})
    };
    
    public static ReviewUpdateDTO ReviewUpdateDTO = new()
    {
        Comment = "Test new",
        Ratings = new Ratings(new []{1.0,2.0,3.0,4.0,5.0,5.0})
    };
}