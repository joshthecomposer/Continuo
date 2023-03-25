#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContinuoClient.Models;
public class PracticeSession
{
    [Key]
    public int PracticeSessionId { get; set; }
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(2, ErrorMessage = "At least 2 characters.")]
    public string Title { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    //Foreign keys
    public int UserId { get; set; }
    //Relationships
    public User? User { get; set; }
    public List<SessionsTasks>? SessionsTasks = new List<SessionsTasks>();
}