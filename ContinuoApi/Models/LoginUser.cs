#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContinuoApi.Models;
public class LoginUser
{
    [Required(ErrorMessage = "Field is required.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Field is required.")]
    public string Password { get; set; }
}