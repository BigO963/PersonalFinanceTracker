using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalTracker.Web.Models.View_Models;

namespace PersonalTracker.API.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<IdentityUser> userManager,  ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        var user = new IdentityUser
        {
            UserName = model.Username,
            Email = model.Email,
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        return Created();
    }
    
    [HttpGet("getId")]
    [Authorize]
    public IActionResult GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        
        if (string.IsNullOrEmpty(userId))
            return BadRequest(new {error = "User ID not found in token"});
        
        return Ok(new {userId});
    }
}