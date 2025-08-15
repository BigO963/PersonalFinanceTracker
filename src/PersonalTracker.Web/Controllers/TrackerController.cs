using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalTracker.Core.Models.DTOS;
using PersonalTracker.Web.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        _logger.LogInformation("Token: " + token);
        
        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        
        var response = await client.GetAsync("http://localhost:5299/auth/getId");
        
        var responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction("Login", "Account");
        }
        
        var dto = JsonSerializer.Deserialize<UserIdDTO>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        string userId = dto.UserId;
        HttpContext.Session.SetString("UserId", userId);
        ViewData["UserId"] = HttpContext.Session.GetString("UserId");
        
        var AccountsPayload = await client.GetAsync($"http://localhost:5299/GetFinancialAccounts?id={userId}");

        if (!AccountsPayload.IsSuccessStatusCode) 
            return View();
        
        var AccountsJson = await AccountsPayload.Content.ReadAsStringAsync();
        
        var accounts = JsonSerializer.Deserialize<List<FinancialAccountDTO>>(
            AccountsJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (TempData["ValidationErrors"] != null)
        {
            ViewData["ShowCreateModal"] = true;

            if (TempData["NewAccount"] is string dtoJson)
            {
                ViewData["NewAccount"] = JsonSerializer.Deserialize<FinancialAccountDTO>(dtoJson);
            }

            // Restore ModelState from TempData
            if (TempData["ModelState"] is string modelStateJson)
            {
                var errors = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(modelStateJson);
                if (errors != null)
                {
                    foreach (var kvp in errors)
                    {
                        foreach (var err in kvp.Value)
                        {
                            ModelState.AddModelError(kvp.Key, err);
                        }
                    }
                }
            }
        }
        
        List<DataPoint> dataPoints = new List<DataPoint>();
        
        var recordData = await client.GetAsync($"http://localhost:5299/getFinancialRecords?userId={userId}");

        if (recordData.IsSuccessStatusCode)
        {
            var json = await recordData.Content.ReadAsStringAsync();
            
            var records = JsonConvert.DeserializeObject<List<FinancialRecordDTO>>(json);
            
            dataPoints = records
                .GroupBy(record => record.Category)
                .Select(g => new DataPoint(g.Key, g.Sum(r => r.Amount)))
                .ToList();
        }
 
        ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

        return View(accounts);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFinanceAccount([FromForm] FinancialAccountDTO financialAccount)
    {
        if (!ModelState.IsValid)
        {
            var errorsDict = ModelState
                .Where(kvp => kvp.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

            TempData["ValidationErrors"] = true;
            TempData["NewAccount"] = JsonSerializer.Serialize(financialAccount);
            TempData["ModelState"] = JsonSerializer.Serialize(errorsDict);

            return RedirectToAction("Index");
        }
        
        var client = _httpClientFactory.CreateClient();
        
        var content = new StringContent(
            JsonSerializer.Serialize(financialAccount),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        
        var result = await client.PostAsync("http://localhost:5299/createFinanceAccount", content);
        
        if (!result.IsSuccessStatusCode)
        {
            return BadRequest("API call failed: " +
                              result.StatusCode + " " +
                              result.ReasonPhrase + await result.Content.ReadAsStringAsync());
        }
        
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> GetFinanceAccounts(string userId)
    {
        return View();
    }

    public async Task<IActionResult> AccountDetails(string id)
    {
        var  client = _httpClientFactory.CreateClient();
        
        var accountPayload = await client.GetAsync($"http://localhost:5299/getFinancialAccount?id={id}");
        
        if (!accountPayload.IsSuccessStatusCode) 
            return View();
        
        var AccountJson = await accountPayload.Content.ReadAsStringAsync();
        
        var account = JsonSerializer.Deserialize<FinancialAccountDTO>(
            AccountJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        
        return View(account);
    }
}