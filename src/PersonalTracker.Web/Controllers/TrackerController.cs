using System.Net.Http.Headers;
using System.Security.Claims;
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
        
        ViewData["dto"] = dto.UserId;
        string userId = dto.UserId;
        _logger.LogInformation($"UserId: {userId}");
        
        var AccountsPayload = await client.GetAsync($"http://localhost:5299/GetFinancialAccounts?id={userId}");
        

        if (!AccountsPayload.IsSuccessStatusCode) 
            return View();
        
        var AccountsJson = await AccountsPayload.Content.ReadAsStringAsync();
        
        var accounts = JsonSerializer.Deserialize<List<FinancialAccountDTO>>(
            AccountsJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        _logger.LogInformation($"Accounts: {accounts}");
        
        return View(accounts);
    }

    
    public async Task<IActionResult> CreateFinanceAccount()
    {
        return View();
    }
    
    // [HttpPost]
    // public async Task<IActionResult> CreateFinanceAccount([FromForm] FinancialAccount financialAccount)
    // {
    //     var client = _httpClientFactory.CreateClient();
    //     
    //     var result = await client.PostAsync("http://localhost:5299/Tracker/getFinancialAccounts", );
    //     
    //     if (!result.IsSuccessStatusCode)
    //         
    //     
    //     return View();
    // }

    
    public async Task<IActionResult> GetFinanceAccounts(string userId)
    {
        return View();
    }
}