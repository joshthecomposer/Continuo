#pragma warning disable CS8618
#pragma warning disable CS8603
#pragma warning disable CS8602
#pragma warning disable CS8600
#pragma warning disable CS8604
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using ContinuoClient.Models;
using System.Text.Json;
using System.Net.Http.Headers;

namespace ContinuoClient.Controllers;
public class ClientAuthController
{
    NavigationManager _nav;
    ProtectedLocalStorage _storage;
    private readonly IConfiguration _config;
    public HttpClient HTTP { get; set; }
    public ClientAuthController(HttpClient http, IConfiguration config, ProtectedLocalStorage storage, NavigationManager nav)
    {
        _config = config;
        _storage = storage;
        _nav = nav;
        HTTP = http;
    }

    public async Task<UserAuthModel> CheckStorage()
    {
        var id = await _storage.GetAsync<int>(_config["JWTSettings:SecretKey"]!, "UserId");
        var token = await _storage.GetAsync<string>(_config["JWTSettings:SecretKey"]!, "AccessToken");
        var refresh = await _storage.GetAsync<string>(_config["JWTSettings:SecretKey"]!, "RefreshToken");
        if (!id.Success || !token.Success || !refresh.Success)
        {
            await Logout();
            return null;
        }
        return new UserAuthModel( id.Value, true, token.Value, refresh.Value);
    }

    public async Task Logout()
    {
        await _storage.DeleteAsync("UserId");
        await _storage.DeleteAsync("AccessToken");
        await _storage.DeleteAsync("RefreshToken");
        _nav.NavigateTo("/login", true);
    }

    public async Task<UserWithToken> ApiLogin(LoginUser loginUser)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };
    HttpRequestMessage request = new(HttpMethod.Post, "http://localhost:5248/api/auth/login");

        request.Content = new StringContent(JsonSerializer.Serialize(loginUser, serializerOptions));
        request.Content.Headers.ContentType
            = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await HTTP.SendAsync(request);

        if(!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong with your login request: Response.");
        }
        string body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);
        UserWithToken result = JsonSerializer.Deserialize<UserWithToken>(body, serializerOptions)!;
        return result;
    }
}