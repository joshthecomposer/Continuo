#pragma warning disable CS8618
#pragma warning disable CS1998
#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
using Microsoft.AspNetCore.Mvc;
using ContinuoApi.Models;
using ContinuoApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using ContinuoApi.Controllers;

namespace ContinuoApi.Controllers;
[Authorize]
[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{

    private readonly IConfiguration _config;

    private DBContext db;
    public UserController(DBContext context, IConfiguration config)
    {
        db = context;
        _config = config;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserNoPassword>> GetById(int id)
    {
        bool IsValid = AuthController.VerifyClaim(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]), id);
        if (!IsValid)
        {
            return Unauthorized();
        }
        User user = await db.Users.FindAsync(id);

        if (user == null)
        {
            return BadRequest("Something went wrong, claim was valid but user was not found");
        }
        return new UserNoPassword(user);
    }
}