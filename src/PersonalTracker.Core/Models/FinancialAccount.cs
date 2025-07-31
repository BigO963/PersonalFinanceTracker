using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace PersonalTracker.Core.Models;

public class FinancialAccount
{
    [Key]
    public Guid account_id { get; set; }
    
    [Required(ErrorMessage = "Account name is required")]
    public string accountName { get; set; }
    
    [Required(ErrorMessage = "Account type is required")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TYPES type { get; set; }
    
    [Required(ErrorMessage = "Initial value is required")]
    [Range(0,999999, ErrorMessage = "Initial value cannot be lower than 0")]
    public double initialValue { get; set; }
    
    [Required(ErrorMessage = "Currency type is required")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CURRENCY currencyType { get; set; }
    
    [Required] 
    public string? UserId { get; set; }

    [ForeignKey(nameof(UserId))] 
    public IdentityUser? user { get; set; }

    
    public enum TYPES
    {
        General,
        Savings,
        Cash,
        Current_Account,
        Credit,
        Debit,
        Bonus,
        Insurance,
        Investment,
        Loan,
        Mortgage,
        Overdraft
    }
    
    public enum CURRENCY
    {
        EUR,
        USD,
        CAD,
        JPY,
        KRW,
        CNY
    }
}