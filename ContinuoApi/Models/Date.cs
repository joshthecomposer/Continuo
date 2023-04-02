#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContinuoApi.Models;
public class Date {
    [Key]
    public int DateId { get; set; }
    [Required]
    public DateTime ScheduledFor { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}