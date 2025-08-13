using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace PersonalTracker.Core.Models.DTOS;

public class FinancialAccountDTO

{
    public Guid account_Id { get; set; }
    
    [Required(ErrorMessage = "Account name is required")]
    public string accountName { get; set; }

    [Required(ErrorMessage = "Account type is required")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FinancialAccount.TYPES type { get; set; }

    [Required(ErrorMessage = "Initial value is required")] 
    public int initialValue { get; set; }

    [Required(ErrorMessage = "Currency type is required")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FinancialAccount.CURRENCY currencyType { get; set; }
    
    [Required] 
    public string UserId { get; set; }

}