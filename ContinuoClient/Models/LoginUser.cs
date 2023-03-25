#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace ContinuoClient.Models;
public class LoginUser
{
    [Required(ErrorMessage = "Field is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "At least 8 characters.")]
    public string Password { get; set; }
}