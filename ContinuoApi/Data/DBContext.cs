#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
using ContinuoApi.Models;

namespace ContinuoApi.Data;
public class DBContext : DbContext 
{   
    public DBContext(DbContextOptions options) : base(options) { }      
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PracticeTask> PracticeTasks { get; set; }
    public DbSet<PracticeSession> PracticeSessions { get; set; }
    public DbSet<SessionsTasks> SessionsTasks { get; set; }
    public DbSet<Instrument> Instruments { get; set; }
}