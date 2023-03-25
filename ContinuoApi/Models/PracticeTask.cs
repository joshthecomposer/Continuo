#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContinuoApi.Models;
public class PracticeTask
{
    [Key]
    public int PracticeTaskId { get; set; }
    [MinLength(20, ErrorMessage = "At least 20 characters.")]
    [Required(ErrorMessage = "Field is required.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Field is required.")]
    public int Minutes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    //Foreign Keys
    public int UserId { get; set; }
    public int InstrumentId { get; set; }
    //Associated Entities
    public User? User { get; set; }
    public Instrument? Instrument { get; set; }
    public List<SessionsTasks>? SessionsTasks { get; set; } = new List<SessionsTasks>();
}

