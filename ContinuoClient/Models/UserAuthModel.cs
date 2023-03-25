#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContinuoClient.Models;
public class UserAuthModel
{
    public int UserId { get; set; }
    public bool Authorized { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public UserAuthModel(int id, bool authed, string access, string refresh)
    {
        UserId = id;
        Authorized = authed;
        AccessToken = access;
        RefreshToken = refresh;
    }
}