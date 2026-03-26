using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Suelos.API.Helpers;
using Suelos.Shared.DTOs;
using Suelos.Shared.Entities;
using Suelos.Shared.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Suelos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(IUserHelper userHelper, IConfiguration configuration) : ControllerBase
{
    private readonly IUserHelper _userHelper = userHelper;
    private readonly IConfiguration _configuration = configuration;

    [AllowAnonymous]
    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] LoginDTO model)
    {
        var user = new User
        {
            Document = Guid.NewGuid().ToString("N")[..10],
            FirstName = "Usuario",
            LastName = "Suelos",
            Email = model.Email,
            UserName = model.Email,
            Address = "Pendiente",
            UserType = UserType.Analyst
        };

        var result = await _userHelper.AddUserAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
        return Ok("Usuario creado correctamente.");
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO model)
    {
        var user = await _userHelper.GetUserAsync(model.Email);
        if (user is null)
        {
            return BadRequest("Email o contrasena incorrectos.");
        }

        var result = await _userHelper.LoginAsync(model);
        if (!result.Succeeded)
        {
            return BadRequest("Email o contrasena incorrectos.");
        }

        return Ok(BuildToken(user));
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Get()
    {
        var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    private TokenDTO BuildToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email!),
            new(ClaimTypes.Role, user.UserType.ToString()),
            new("Document", user.Document),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddDays(30);
        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new TokenDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}
