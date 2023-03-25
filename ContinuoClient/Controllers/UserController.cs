#pragma warning disable CS8618
#pragma warning disable CS8603
#pragma warning disable CS8602
#pragma warning disable CS8600
#pragma warning disable CS8604
using ContinuoClient.Models;
using ContinuoClient.Utilities;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;

namespace ContinuoClient.Controllers;
public class UserController
{
    NavigationManager _nav;
    ProtectedLocalStorage _storage;
    public HttpClient HTTP { get; set; }
    private readonly IConfiguration _config;
    public UserController(HttpClient http, IConfiguration config, ProtectedLocalStorage storage, NavigationManager nav)
    {
        HTTP = http;
        _config = config;
        _storage = storage;
        _nav = nav;
    }

    public async Task<UserWithToken> GetUserById(int id, string token)
    {
        HttpRequestMessage request = new(HttpMethod.Get, "http://localhost:5248/api/users/"+id);
        request.Headers.Authorization
            = new AuthenticationHeaderValue("Bearer " + token);
        request.Content.Headers.ContentType
            = new MediaTypeHeaderValue("application/json");
        
        HttpResponseMessage response = await HTTP.SendAsync(request);

        if(!response.IsSuccessStatusCode)
        {
            return null;
        }
        return null;
    }

    public async Task<UserWithToken> ApiCreateUserAsync(User user)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };

        HttpRequestMessage request = new(HttpMethod.Post, "http://localhost:5248/api/auth/register");
        request.Content = new StringContent(JsonSerializer.Serialize(user, serializerOptions));
        request.Content.Headers.ContentType
            = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await HTTP.SendAsync(request);

        if(!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong with your login request: Response.");
        }
        string body = await response.Content.ReadAsStringAsync();
        UserWithToken result = JsonSerializer.Deserialize<UserWithToken>(body, serializerOptions)!;
        return result;
    }

    public async Task<RefreshRequest> TryRefreshToken(RefreshRequest rf)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };

        HttpRequestMessage request = new(HttpMethod.Post, "http://localhost:5248/api/auth/tokens/refresh");
        request.Content = new StringContent(JsonSerializer.Serialize(rf, serializerOptions));
        request.Content.Headers.ContentType
            = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await HTTP.SendAsync(request);

        if(!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong with your login request: Response.");
        }
        string body = await response.Content.ReadAsStringAsync();
        RefreshRequest result = JsonSerializer.Deserialize<RefreshRequest>(body, serializerOptions)!;
        return result;
    }
}