using Xunit.Abstractions;

namespace Application.API.IntegrationTests;

public class UserControllerTests : IntegrationTest
{
    public UserControllerTests(ITestOutputHelper output) : base(output)
    {
        
    }
    
    [Fact]
    public void Test1()
    {
        // Arrange
        // Act
        // Assert
        Assert.True(true);
    }
}