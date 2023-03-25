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
[Route("api/instruments")]
[ApiController]
public class InstrumentController : ControllerBase
{

    private readonly IConfiguration _config;

    private DBContext db;
    public InstrumentController(DBContext context, IConfiguration config)
    {
        db = context;
        _config = config;
    }

    [HttpPost("create")]
    public async Task<ActionResult<Instrument>> CreateInstrument([FromBody] Instrument inst)
    {
        foreach(var h in Request.Headers)
        {
            Console.Write(h + ", ");
        }
        bool IsValid = AuthController.VerifyClaim(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]), inst.UserId);
        if (!IsValid)
        {
            Console.WriteLine("Token was invalid for some reason....");
            return Unauthorized();
        }
        db.Add(inst);
        await db.SaveChangesAsync();
        return CreatedAtAction(
                nameof (GetCreatedInstrumentAsync),
                new {id = inst.InstrumentId},
                inst
            );
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [ActionName(nameof(GetCreatedInstrumentAsync))]
    public async Task<ActionResult<PracticeTask>> GetCreatedInstrumentAsync(int id)
    {
        PracticeTask pt = await db.PracticeTasks.FindAsync(id);
        if (pt != null)
        {
            return pt;
        }
        return BadRequest("Something went wrong when attempting to save the Practice Task in the database.");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<List<Instrument>>> GetAllByUserId(int id)
    {
        Console.WriteLine("Recieving request for user instrument list...");
        bool IsValid = AuthController.VerifyClaim(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]), id);
        if (!IsValid)
        {
            Console.WriteLine("Claim verification failed, user is unauthorized...");
            return Unauthorized();
        }

        List<Instrument> result = db.Instruments.Where(i => i.UserId == id).ToList();
        Console.WriteLine("Result of instrument request: ");
        result.ForEach(r => Console.Write(r.Name + " "));
        Console.Write("\n");
        return result;
    }
}