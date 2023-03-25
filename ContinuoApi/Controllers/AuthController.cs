#pragma warning disable CS8618
#pragma warning disable CS1998
#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8602
using Microsoft.AspNetCore.Mvc;
using ContinuoApi.Data;
using ContinuoApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using ContinuoApi.Utilities;

namespace ContinuoApi.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    private DBContext db;
    public AuthController(DBContext context, IConfiguration config)
    {
        db = context;
        _config = config;
    }


//============REGISTER AND CONFIRM CREATED USER=============

    [HttpPost("register")]
    public async Task<ActionResult<User>> Create([FromBody] User newUser)
    {
        if (ModelState.IsValid)
        {
            User check = await db.Users.Where(u => u.Email == newUser.Email).FirstOrDefaultAsync();
            if (check != null)
            {
                return BadRequest("The specified user already exists.");
            }
            // string key = "b14ca5898a4e4133bbce2ea2315a1916";
            // newUser.Password = AesOperation.DecryptString(key, newUser.Password);
            // newUser.Confirm = AesOperation.DecryptString(key, newUser.Confirm);

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);
            db.Add(newUser);
            await db.SaveChangesAsync();
            return CreatedAtAction(
                nameof (GetCreatedUserAsync),
                new {id = newUser.UserId},
                new UserNoPassword(newUser)
            );
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [ActionName(nameof(GetCreatedUserAsync))]
    public async Task<ActionResult<User>> GetCreatedUserAsync(int id)
    {
        User user = await db.Users.FindAsync(id);
        if (user != null)
        {
            return user;
        }
        return BadRequest("Something went wrong when attempting to save the user in the database.");
    }

//=====================================================
//====================LOGIN USER=======================

    [HttpPost("login")]
    public async Task<ActionResult<UserWithToken>> Login(LoginUser loginUser)
    {
        string error = "Email or password are incorrect.";
        //TODO: REENABLE THIS DECRYPTION WHEN WORKING WITH CLIENT.
        // loginUser.Password = AesOperation.DecryptString(_config["EncryptionKeys:Main"], loginUser.Password);
        // Console.WriteLine("Decrypted password is: " + loginUser.Password);
        if (ModelState.IsValid)
        {
            User check = await db.Users.Where(u => u.Email == loginUser.Email).SingleOrDefaultAsync();
            if (check == null)
            {
                return BadRequest(error);
            }
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            PasswordVerificationResult hashCheck = hasher.VerifyHashedPassword(loginUser, check.Password, loginUser.Password);
            if (hashCheck == 0)
            {
                return BadRequest(error);
            }
            RefreshToken newRefreshToken = GenerateRefreshToken(check.UserId);
            db.RefreshTokens.Add(newRefreshToken);
            await db.SaveChangesAsync();
            string token = GenerateAccessToken(check.UserId);

            return new UserWithToken(check, token, newRefreshToken.Value);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
//=============================================================
//=========JWT GENERATION VALIDATION AND REFRESH===============

    //TODO: Make everything here async.
    private string GenerateAccessToken(int userId)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_config["JWTSettings:SecretKey"]);
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, Convert.ToString(userId))
            }),
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(int userId)
    {
        RefreshToken rt = new();

        byte[] rn = new byte[32];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(rn);
            rt.Value = Convert.ToBase64String(rn);
        }
        rt.UserId = userId;
        rt.ExpiryDate = DateTime.UtcNow.AddMonths(6);
        return rt;
    }

    private bool ValidateRefreshToken(User user, string refreshToken)
    {
        RefreshToken refreshTokenUser = db.RefreshTokens.Where(rt => rt.Value == refreshToken)
                                            .OrderByDescending(rt => rt.ExpiryDate)
                                            .FirstOrDefault();
        if (refreshTokenUser != null && refreshTokenUser.UserId == user.UserId && refreshTokenUser.ExpiryDate > DateTime.UtcNow)
        {
            return true;
        }
        return false;
    }

    [HttpPost("tokens/refresh")]
    public async Task<ActionResult<RefreshRequest>> DoRefreshToken([FromBody] RefreshRequest refreshRequest)
    {
        //TODO: Make logic in client that tries to refresh token if something fails?
        if (ModelState.IsValid)
        {
            string accessToken = refreshRequest.AccessToken;
            string refreshToken = refreshRequest.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                BadRequest("Invalid access token or refresh token");
            }
            int id = Int32.Parse(principal.Identity.Name);
            var user = await db.Users.Where(u => u.UserId == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("User not found for this token.");
            }

            RefreshToken refreshTokenUser = db.RefreshTokens.Where(rt => rt.Value == refreshToken)
                                                            .OrderByDescending(rt => rt.ExpiryDate)
                                                            .FirstOrDefault();
            if (refreshTokenUser != null && refreshTokenUser.UserId == user.UserId && refreshTokenUser.ExpiryDate > DateTime.UtcNow)
            {
                var newToken = GenerateAccessToken(user.UserId);
                RefreshRequest result = new(newToken, refreshRequest.RefreshToken);
                return result;
            }
        }
        return BadRequest("Invalid refresh request.");
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:SecretKey"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;

    }

    public static bool VerifyClaim(AuthenticationHeaderValue input, int id)
    {   
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        string credentials = input.Parameter;
        Console.WriteLine("INPUT PARAMETER ON VERIFY CLAIM IS: " + credentials);
        Claim verifiedClaim = handler.ReadJwtToken(credentials).Claims.Where(c=>c.Value == id.ToString()).FirstOrDefault();
        return verifiedClaim != null;
    }

//==================================================

}