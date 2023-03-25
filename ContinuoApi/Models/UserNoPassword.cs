#pragma warning disable CS8618

namespace ContinuoApi.Models;
public class UserNoPassword
{

    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}

    public UserNoPassword(User user)
    {
        UserId = user.UserId;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;

        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
    }
}