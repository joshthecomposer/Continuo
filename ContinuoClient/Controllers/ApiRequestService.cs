#pragma warning disable CS8618
#pragma warning disable CS8603
#pragma warning disable CS8602
#pragma warning disable CS8600
#pragma warning disable CS8604
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using ContinuoClient.Models;
using ContinuoClient.Frameworks;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace ContinuoClient.Controllers;
public class ApiRequestService
{
    public HttpClient HTTP { get; set; }
    NavigationManager _nav;
    ProtectedLocalStorage _storage;
    UserController _userController;
    private readonly IConfiguration _config;
    public ApiRequestService(HttpClient http, IConfiguration config, ProtectedLocalStorage storage, NavigationManager nav, UserController userController)
    {
        _config = config;
        _storage = storage;
        _nav = nav;
        HTTP = http;
        _userController = userController;
    }

    public async Task<object> InitiateGetRequest(string uri, string token, ApiReturnType returnType)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };
        HttpRequestMessage request = new(HttpMethod.Get, uri);

        request.Headers.Authorization
            = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await HTTP.SendAsync(request);

        if(!response.IsSuccessStatusCode)
        {
            return null;
        }
        string body = await response.Content.ReadAsStringAsync();

        switch(returnType)
        {
            case ApiReturnType.User:
                User user = JsonSerializer.Deserialize<User>(body, serializerOptions)!;
                return (object) user;
            case ApiReturnType.InstrumentList:
                List<Instrument> instruments = JsonSerializer.Deserialize<List<Instrument>>(body, serializerOptions)!;
                return (object) instruments;
            case ApiReturnType.PracticeTaskList:
                List<PracticeTask> practiceTasks = JsonSerializer.Deserialize<List<PracticeTask>>(body, serializerOptions)!;
                return (object) practiceTasks;
            case ApiReturnType.PracticeSession:
                PracticeSession practiceSession = JsonSerializer.Deserialize<PracticeSession>(body, serializerOptions)!;
                return (object) practiceSession;
        }
        return null;
    }

    public async Task<object> InitiatePostRequest(string uri, string token, ApiReturnType returnType, object obj)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };
        HttpRequestMessage request = new(HttpMethod.Post, uri);

        Console.WriteLine("Ingoing object is: " + JsonSerializer.Serialize(obj, serializerOptions));
        request.Content = new StringContent(JsonSerializer.Serialize(obj, serializerOptions));
        request.Headers.Authorization
            = new AuthenticationHeaderValue("Bearer", token);
        request.Content.Headers.ContentType
            = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await HTTP.SendAsync(request);

        if(!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ATTEMPT TO FETCH API: "+ response);
            Console.WriteLine((string)await response.Content.ReadAsStringAsync());
            return null;
        }
        string body = await response.Content.ReadAsStringAsync();

        switch(returnType)
        {
            case ApiReturnType.User:
                User user = JsonSerializer.Deserialize<User>(body, serializerOptions)!;
                return (object) user;
            case ApiReturnType.InstrumentList:
                List<Instrument> instruments = JsonSerializer.Deserialize<List<Instrument>>(body, serializerOptions)!;
                return (object) instruments;
            case ApiReturnType.PracticeTaskList:
                List<PracticeTask> practiceTasks = JsonSerializer.Deserialize<List<PracticeTask>>(body, serializerOptions)!;
                return (object) practiceTasks;
            case ApiReturnType.PracticeSession:
                PracticeSession practiceSession = JsonSerializer.Deserialize<PracticeSession>(body, serializerOptions)!;
                return (object) practiceSession;
        }
        return null;
    }
}