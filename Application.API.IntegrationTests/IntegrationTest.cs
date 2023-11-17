using System.Net.Http.Json;
using ApplicationCommon.DTOs.User;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Application.API.IntegrationTests;

public class IntegrationTest
{
    protected readonly HttpClient TestClient;
    protected readonly ITestOutputHelper _output;
    
    public IntegrationTest(ITestOutputHelper output)
    {
        _output = output;
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    
                });
            });
        TestClient = appFactory.CreateClient();
        TestClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetJwtToken().Result);
    }

    protected async Task<Guid> GetUserId()
    {
        _output.WriteLine(TestClient.DefaultRequestHeaders.Authorization.Parameter);
        var response = await TestClient.GetFromJsonAsync<UserDTO>("http://localhost:5241/api/User/fromToken");
        return response.Id;
    }

    private async Task<string?> GetJwtToken()
    {
        await TestClient.PostAsJsonAsync("http://localhost:5241/api/Auth/register", new RegisterUserDTO()
        {
            Email = "testEmail",
            Password = "testPassword",
            Name = "testFirstName",
            PhoneNumber = "testPhoneNumber",
            Address = "testAddress",
            Description = "testDescription"
        });
        var response = TestClient.PostAsJsonAsync("http://localhost:5241/api/Auth/login", new LoginUserDTO()
        {
            Email = "testEmail",
            Password = "testPassword"
        }).Result.Content;
        _output.WriteLine("asdasd" +  await response.ReadAsStringAsync());
        var responseContent = await response.ReadFromJsonAsync<AuthUser>();
        return responseContent?.Token;
    }
}