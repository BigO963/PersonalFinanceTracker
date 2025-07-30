using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalTracker.Core.Models;

public class FinancialRecord
{
    [Key]
    public int record_id { get; set; }

    public string category { get; set; }

    public double amount { get; set; }

    public string currency { get; set; }

    public DateOnly date { get; set; }
    
    public TimeOnly time { get; set; }
    
    public Guid AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public FinancialAccount Account { get; set; }
    
    //TODO: add more properties like notes, labels, payee, payment type, status, places where the transaction took place and attachments
}