#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContinuoApi.Models;
public class SessionDate
{
    [Key]
    public int SessionDateId { get; set; }
    //TODO: Is this the best place for this??
    public int Status { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    //Foreign Keys
    public int PracticeSessionId { get; set; }
    public int DateId { get; set; }
    //Associated Entitites
    public PracticeSession PracticeSession{ get; set; }
    public Date Date { get; set; }
}