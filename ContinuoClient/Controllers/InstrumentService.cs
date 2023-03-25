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
public class InstrumentService
{
    public HttpClient HTTP { get; set; }
    private readonly IConfiguration _config;
    public InstrumentService(HttpClient http, IConfiguration config, UserController userController)
    {
        HTTP = http;
        _config = config;
    }

    public async Task<Instrument> CreateInstrumentAsync(Instrument inst, string token)
    {
        //TODO: REQUEST LOGIC SHOULD BE PUT IN ONE FUNCTION TAKING THE URI AS AN EXTRA ARGUMENT.
        Console.WriteLine("Client Incoming instrument name: " + inst.Name);
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };
        HttpRequestMessage request = new(HttpMethod.Post, "http://localhost:5248/api/instruments/create");

        request.Content = new StringContent(JsonSerializer.Serialize(inst, serializerOptions));
        request.Headers.Authorization
            = new AuthenticationHeaderValue("Bearer", token);
        request.Content.Headers.ContentType
            = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await HTTP.SendAsync(request);

        if(!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ATTEMPT TO FETCH API: "+ response);
            return null;
        }
        string body = await response.Content.ReadAsStringAsync();

        Instrument result = JsonSerializer.Deserialize<Instrument>(body, serializerOptions)!;
        return result;
    }
}