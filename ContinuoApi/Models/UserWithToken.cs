#pragma warning disable CS8618

namespace ContinuoApi.Models;
public class UserWithToken
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}

    public UserWithToken(User user, string token, string rfToken)
    {
        AccessToken = token;
        RefreshToken = rfToken;
        UserId = user.UserId;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;

        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
    }
}