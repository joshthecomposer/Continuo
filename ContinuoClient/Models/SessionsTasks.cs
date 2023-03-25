#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContinuoClient.Models;
public class SessionsTasks
{
    [Key]
    public int SessionsTasksId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    //Foreign Keys
    public int PracticeSessionId { get; set; }
    public int PracticeTaskId { get; set; }
    //Associated Entities
    public PracticeSession? PracticeSession { get; set; }
    public PracticeTask? PracticeTask { get; set; }
}