#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContinuoApi.Models;
public class RefreshRequest
{
    [Required]
    public string AccessToken { get; set; }
    [Required]
    public string RefreshToken { get; set; }

    public RefreshRequest(string token, string refreshToken)
    {
        AccessToken = token;
        RefreshToken = refreshToken;
    }

    [JsonConstructor]
    public RefreshRequest(){}
}