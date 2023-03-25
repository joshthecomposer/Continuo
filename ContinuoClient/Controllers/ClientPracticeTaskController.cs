#pragma warning disable CS8618
#pragma warning disable CS8603
#pragma warning disable CS8602
#pragma warning disable CS8600
#pragma warning disable CS8604
using ContinuoClient.Models;
using ContinuoClient.Utilities;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ContinuoClient.Controllers;
public class ClientPracticeTaskController
{
    public HttpClient HTTP { get; set; }
    private readonly IConfiguration _config;
    public ClientPracticeTaskController(HttpClient http, IConfiguration config, UserController userController)
    {
        HTTP = http;
        _config = config;
    }
}