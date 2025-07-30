using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace PersonalTracker.Core.Models.DTOS;

public class FinancialAccountDTO
{
    [Required]
    public string accountName { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FinancialAccount.TYPES type { get; set; }
    
    [Required]
    public int initialValue { get; set; }
    
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FinancialAccount.CURRENCY currencyType { get; set; }
    
    [Required] 
    public string UserId { get; set; }

}