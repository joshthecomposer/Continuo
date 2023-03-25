#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContinuoApi.Models;
public class User
{
    [Key]
    public int UserId { get; set; }
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(2, ErrorMessage = "At least two characters.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(2, ErrorMessage = "At least two characters.")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Field is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "At least 8 characters.")]
    public string Password { get; set; }
    [NotMapped]
    [Required(ErrorMessage = "Field is required.")]
    [Compare("Password", ErrorMessage = "Passwords did not match.")]
    public string Confirm { get; set; }
    //TODO: Put all createds/updateds into a base class that each class can inherit.
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    //Relationships
    public List<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();
    public List<PracticeTask>? PracticeTasks { get; set; } = new List<PracticeTask>();
    public List<PracticeSession>? PracticeSessions { get; set; } = new List<PracticeSession>();
    public List<Instrument>? Instruments { get; set; } = new List<Instrument>();
}