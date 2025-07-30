using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PersonalTracker.Web.Models;
using PersonalTracker.Web.Models.View_Models;
using System.IdentityModel.Tokens.Jwt;

namespace PersonalTracker.Web.Controllers;

public class Account : Controller
{
    
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<Account> _logger;

    public Account(IHttpClientFactory httpClientFactory,  ILogger<Account> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    
    // GET
    public IActionResult register()
    {
        return View();
    }
    
    // POST
    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
    {
        if (!ModelState.IsValid) 
            return View(model);
        
        var httpClient = _httpClientFactory.CreateClient();
        
        var account = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        
        using var resoponse =
           await httpClient.PostAsync($"http://localhost:5299/auth/register", account);

        if (resoponse.IsSuccessStatusCode)
            return RedirectToAction("login");
        
        var status = resoponse.StatusCode;
        var content = await resoponse.Content.ReadAsStringAsync();
        
        if (status == HttpStatusCode.BadRequest)
            ModelState.AddModelError(string.Empty, "Invalid registration data.");
        
        else if (status == HttpStatusCode.Conflict)
            ModelState.AddModelError(string.Empty, "Account already exists.");
        
        else ModelState.AddModelError(string.Empty, $"Error { (int)status }: {status}");
        
        return View(model);
    }
    
    
    // GET
    public IActionResult login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        var httpClient = _httpClientFactory.CreateClient();
        
        var content = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        
        var response = await httpClient.PostAsync($"http://localhost:5299/login", content);
        
        var raw = await response.Content.ReadAsStringAsync();

        var token = JsonSerializer.Deserialize<TokenDTO>(raw);

        if (User.Identity.IsAuthenticated)
        {
            HttpContext.Session.SetString("AccessToken", token.AccessToken);
            return RedirectToAction("Index", "Tracker");
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, model.Email),
        };
        
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(principal, new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) });
        
        HttpContext.Session.SetString("AccessToken", token.AccessToken);

        return RedirectToAction("Index", "Tracker");
    }
        
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        HttpContext.Session.Clear();

        foreach (var cookie in Request.Cookies.Keys.Where(k => k.StartsWith(".AspNetCore")))
        {
            Response.Cookies.Delete(cookie);
        }
        
        return RedirectToAction("Login", "Account");
            
    } 
}

