using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Application.API.IntegrationTests;

public class IntegrationTest
{
    protected readonly HttpClient TestClient;
    
    public IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    
                });
            });
        TestClient = appFactory.CreateClient();
    }
}