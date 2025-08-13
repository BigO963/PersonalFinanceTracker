using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalTracker.API.Context;
using PersonalTracker.Core.Models;
using PersonalTracker.Core.Models.DTOS;

namespace PersonalTracker.API.Controllers;

[ApiController]
public class TrackerController : Controller
{
    
    private readonly FinanceDbContext _context;

    public TrackerController(FinanceDbContext context)
    {
        _context = context;
    } 
    
    [HttpPost("createFinanceAccount")]
    public async Task<IActionResult> CreateFinanceAccount([FromBody] FinancialAccountDTO accountDTO)
    {
        if (ModelState.IsValid)
        {
            var account = new FinancialAccount
            {
                accountName = accountDTO.accountName,
                type = accountDTO.type,
                initialValue = accountDTO.initialValue,
                currencyType = accountDTO.currencyType,
                UserId = accountDTO.UserId,
            };
            
            _context.Add(account);
            await _context.SaveChangesAsync();
            return Created();
        }
        return BadRequest(ModelState);
    }
    
    [HttpGet("getFinancialAccounts")]
    public async Task<IActionResult> GetFinancialAccounts(string id)
    {
       var accounts = await _context.Accounts
            .Where(account => account.UserId == id)
            .ToArrayAsync();
       
       return Ok(accounts);
    }
    
    [HttpPost("createFinanceRecord")]
    public async Task<IActionResult> CreateFinanceRecord([FromBody] FinancialRecordDTO recordDTO)
    {
        if (ModelState.IsValid)
        {
            var record = new FinancialRecord
            {
                category = recordDTO.Category,
                amount = recordDTO.Amount,
                currency = recordDTO.Currency,
                date = recordDTO.Date,
                time = recordDTO.Time,
                AccountId = recordDTO.AccountId,
            };
            
            _context.Add(record);
            await _context.SaveChangesAsync();
            return Created();
        }
        return BadRequest(ModelState);
    }

    [HttpGet("getFinancialRecords")]
    public async Task<IActionResult> GetFinancialRecords(Guid id)
    {
        var records = await _context.Records
            .Where(record => record.AccountId == id)
            .ToArrayAsync();
       
        return Ok(records);
    }
    
    
}
