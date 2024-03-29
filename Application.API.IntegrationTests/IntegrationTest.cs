using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using Application.API.IntegrationTests.ServiceMocks;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.DbHelper;
using ApplicationLogic.BackgroundServices;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Bson;
using StackExchange.Redis;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Application.API.IntegrationTests;

public class IntegrationTest
{
    
    protected readonly HttpClient TestClient;
    protected readonly ITestOutputHelper _output;
    protected const string BaseUrl = "http://localhost:5241/";
    
    public IntegrationTest(ITestOutputHelper output)
    {
        _output = output;
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(IMongoDbContext));
                    services.RemoveAll(typeof(IConnectionMultiplexer));
                    services.RemoveAll(typeof(BackgroundService));
                    services.AddSingleton<IConnectionMultiplexer>(new ConnectionMultiplexerMock());
                    
                    services.AddSingleton<IMongoDbContext>(_ =>
                    {
                        var connectionString = "mongodb+srv://compassuser:wBzZ4kD5ejcI1FWf@democluster.4nn3xhe.mongodb.net/";
                        var databaseName = "test";
                        return new MongoDbContext(connectionString, databaseName);
                    });
                    CollectionGetter.Initialize(services.BuildServiceProvider().GetService<IMongoDbContext>()!);
                });
            });
        TestClient = appFactory.CreateClient();
        TestClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetJwtToken());
    }
    
    
    protected async Task<HttpResponseMessage> Get<T>(string endpoint)
    {
        var response = await TestClient.GetAsync(BaseUrl + endpoint);
        _output.WriteLine("================status===============\n");
        _output.WriteLine("GET  "+BaseUrl + endpoint);
        _output.WriteLine(response.StatusCode.ToString());
        _output.WriteLine("================body===============\n");
        _output.WriteLine((await response.Content.ReadAsStringAsync()));
        return response;
    }
    
    protected async Task<T> GetObjectFromResponse<T>(HttpResponseMessage response)
    {
        return await response.Content.ReadFromJsonAsync<T>();
    }
    
    protected async Task<HttpResponseMessage> Post<T>(string endpoint, object body)
    {
        var response = await TestClient.PostAsJsonAsync(BaseUrl + endpoint, body);
        _output.WriteLine("================status===============\n");
        _output.WriteLine("POST  " + BaseUrl+ endpoint);
        _output.WriteLine(response.StatusCode.ToString());
        _output.WriteLine("================body===============\n");
        _output.WriteLine((await response.Content.ReadAsStringAsync()));
        return response;
    }
    
    protected async Task<Guid> GetIdFromResponse(HttpResponseMessage response)
    {
        return new Guid((await response.Content.ReadAsStringAsync()).Trim('"'));
    } 
    
    protected async Task<HttpResponseMessage> Put<T>(string endpoint, object body)
    {
        var response = await TestClient.PutAsJsonAsync(BaseUrl + endpoint, body);
        _output.WriteLine("================status===============\n");
        _output.WriteLine("PUT  " + BaseUrl+ endpoint);
        _output.WriteLine(response.StatusCode.ToString());
        _output.WriteLine("================body===============\n");
        _output.WriteLine((await response.Content.ReadAsStringAsync()));
        return response;
    }
    
    protected async Task<HttpResponseMessage> Delete(string endpoint)
    {
        var response = await TestClient.DeleteAsync(BaseUrl + endpoint);
        _output.WriteLine("================status===============\n");
        _output.WriteLine("DELETE  " + BaseUrl+ endpoint);
        _output.WriteLine(response.StatusCode.ToString());
        _output.WriteLine("================body===============\n");
        _output.WriteLine((await response.Content.ReadAsStringAsync()));
        return response;
    }

    protected async Task<Guid> GetUserId()
    {
        var response = await TestClient.GetAsync("http://localhost:5241/api/User/fromToken").Result.Content.ReadFromJsonAsync<UserDTO>();
        _output.WriteLine("RESPONSE LOG: " + response);
        return response.Id;
    }

    protected async Task SwitchRole(bool toHost)
    {
        _output.WriteLine(TestClient.DefaultRequestHeaders.Authorization.ToString());
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Auth/switch-role-to-host/" + toHost, default(object));
        var user = await response.Content.ReadFromJsonAsync<AuthUser>();
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
    }

    private string? GetJwtToken()
    {
        var email = "testEmail@mail.com " + new string(Guid.NewGuid().ToString().ToCharArray().Take(5).ToArray());
        var password = "testPassword " + new string(Guid.NewGuid().ToString().ToCharArray().Take(5).ToArray());
        
        var response1 = TestClient.PostAsJsonAsync("http://localhost:5241/api/Auth/register", new RegisterUserDTO()
        {
            Email = email,
            Password = password,
            Name = "testFirstName",
        }).Result;
        
        _output.WriteLine("AAAAAAAAAAA" +response1.Content.ReadAsStringAsync().Result);
        var response = TestClient.PostAsJsonAsync("http://localhost:5241/api/Auth/login", new LoginUserDTO()
        {
            Email = email,
            Password = password
        }).Result;
        _output.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAA" +response.Content.ReadAsStringAsync().Result);
        var responseContent = response.Content.ReadFromJsonAsync<AuthUser>().Result;
        return responseContent.Token;
    }
}