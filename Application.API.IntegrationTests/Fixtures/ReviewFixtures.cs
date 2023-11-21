using ApplicationCommon.DTOs.Review;

namespace Application.API.IntegrationTests.Fixtures;

public static class ReviewFixtures
{
    public static ReviewCreateDTO ReviewCreateDTO = new()
    {
        BookingId = new Guid(),
        Comment = "Test",
        Ratings = new []{1.0}
    };
    
    public static ReviewUpdateDTO ReviewUpdateDTO = new()
    {
        Comment = "Test new",
        Ratings = new []{1.0,2.0,3.0}
    };
}