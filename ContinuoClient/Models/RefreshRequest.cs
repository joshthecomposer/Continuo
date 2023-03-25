#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContinuoClient.Models;
public class RefreshRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public RefreshRequest(string token, string refreshToken)
    {
        AccessToken = token;
        RefreshToken = refreshToken;
    }
    [JsonConstructor]
    public RefreshRequest(){}
}