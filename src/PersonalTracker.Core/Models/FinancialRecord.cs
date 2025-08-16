using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PersonalTracker.Core.Models;

public class FinancialRecord
{
    [Key]
    public Guid record_id { get; set; }

    public string category { get; set; }

    public double amount { get; set; }

    public string currency { get; set; }

    public DateOnly date { get; set; }
    
    public TimeOnly time { get; set; }
    
    [Required]
    public Guid FinancialAccountId { get; set; }
    
    [ForeignKey(nameof(FinancialAccountId))]
    [JsonIgnore]
    public FinancialAccount FinancialAccount { get; set; }
    
    //TODO: add more properties like notes, labels, payee, payment type, status, places where the transaction took place and attachments
}