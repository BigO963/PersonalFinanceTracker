using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalTracker.Core.Models.DTOS;
using PersonalTracker.Web.Models;

namespace PersonalTracker.Web.Controllers;

public class TrackerController : Controller
{
    
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TrackerController> _logger;

    public TrackerController(IHttpClientFactory httpClientFactory,  ILogger<TrackerController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        
        var token = HttpContext.Session.GetString("AccessToken");
        
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        
        var response = await client.GetAsync("http://localhost:5299/auth/getId");
        
        var responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            // Optionally inspect why it failed
            return BadRequest("API call failed: " + responseContent);
        }
        
        var dto = JsonSerializer.Deserialize<UserIdDTO>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        string userId = dto.UserId;
        HttpContext.Session.SetString("UserId", userId);
        
        var AccountsPayload = await client.GetAsync($"http://localhost:5299/GetFinancialAccounts?id={userId}");

        if (!AccountsPayload.IsSuccessStatusCode) 
            return View();
        
        var AccountsJson = await AccountsPayload.Content.ReadAsStringAsync();
        
        var accounts = JsonSerializer.Deserialize<List<FinancialAccountDTO>>(
            AccountsJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        
        return View(accounts);
    }

    
    public async Task<IActionResult> CreateFinanceAccount()
    {
        ViewData["UserId"] = HttpContext.Session.GetString("UserId");
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFinanceAccount([FromForm] FinancialAccountDTO financialAccount)
    {
        var client = _httpClientFactory.CreateClient();
        
        var content = new StringContent(
            JsonSerializer.Serialize(financialAccount),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        
        var result = await client.PostAsync("http://localhost:5299/createFinanceAccount", content);

        if (!result.IsSuccessStatusCode)
            return BadRequest("API call failed: " + result.StatusCode +  " " + result.ReasonPhrase + result.Content);
        
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> GetFinanceAccounts(string userId)
    {
        return View();
    }
}