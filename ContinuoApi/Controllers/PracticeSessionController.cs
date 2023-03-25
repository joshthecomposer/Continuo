#pragma warning disable CS8618
#pragma warning disable CS1998
#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
using Microsoft.AspNetCore.Mvc;
using ContinuoApi.Models;
using ContinuoApi.Data;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace ContinuoApi.Controllers;
[Authorize]
[Route("api/sessions")]
[ApiController]
public class PracticeSessionController : ControllerBase
{

    private readonly IConfiguration _config;

    private DBContext db;
    public PracticeSessionController(DBContext context, IConfiguration config)
    {
        db = context;
        _config = config;
    }

    [HttpPost("tasks/create")]
    public async Task<ActionResult<PracticeTask>> CreateTask([FromBody] PracticeTask pt)
    {
        Console.WriteLine("Incoming PT is: "+pt);
        bool IsValid = AuthController.VerifyClaim(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]), pt.UserId);
        if (!IsValid)
        {
            return Unauthorized();
        }
        db.Add(pt);
        await db.SaveChangesAsync();
        return CreatedAtAction(
                nameof (GetCreatedTaskAsync),
                new {id = pt.PracticeTaskId},
                pt
            );
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [ActionName(nameof(GetCreatedTaskAsync))]
    public async Task<ActionResult<PracticeTask>> GetCreatedTaskAsync(int id)
    {
        PracticeTask pt = await db.PracticeTasks.FindAsync(id);
        if (pt != null)
        {
            return pt;
        }
        return BadRequest("Something went wrong when attempting to save the Practice Task in the database.");
    }

    [HttpGet("tasks/{id}")]
    public async Task<ActionResult<List<PracticeTask>>> GetPracticeTasksByUserId(int id)
    {
        Console.WriteLine("Fetching user practice tasks with userId of: " + id);
        bool IsValid = AuthController.VerifyClaim(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]), id);
        if (!IsValid)
        {
            Console.WriteLine("Verification of claim to practice task list failed... Please send a valid token.");
            return Unauthorized();
        }
        List<PracticeTask> result = db.PracticeTasks.Where(p => p.UserId == id).ToList();
        return result;
    }

    [HttpPost("create")]
    public async Task<ActionResult<PracticeSession>> CreatePracticeSession([FromBody] PracticeSession ps)
    {
        bool IsValid = AuthController.VerifyClaim(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]), ps.UserId);
        if (!IsValid)
        {
            return Unauthorized();
        }
        db.Add(ps);
        await db.SaveChangesAsync();
        return CreatedAtAction(
                nameof (GetCreatedSessionAsync),
                new {id = ps.PracticeSessionId},
                ps
            );
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [ActionName(nameof(GetCreatedSessionAsync))]
    public async Task<ActionResult<PracticeSession>> GetCreatedSessionAsync(int id)
    {
        PracticeSession ps = await db.PracticeSessions.FindAsync(id);
        if (ps != null)
        {
            return ps;
        }
        return BadRequest("Something went wrong when attempting to save the Practice session in the database.");
    }

}