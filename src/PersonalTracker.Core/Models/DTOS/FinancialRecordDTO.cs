using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PersonalTracker.Core.Models.DTOS;

public class FinancialRecordDTO
{
    [Required]
    public required string Category { get; set; }
    [Required]
    public required double Amount { get; set; }
    [Required]
    public required string Currency { get; set; }
    [Required]
    public required DateOnly Date { get; set; }
    [Required]
    public required TimeOnly Time { get; set; }
    
    [Required]
    public Guid AccountId { get; set; }
    

    //TODO: add more properties like notes, labels, payee, payment type, status, places where the transaction took place and attachments

}