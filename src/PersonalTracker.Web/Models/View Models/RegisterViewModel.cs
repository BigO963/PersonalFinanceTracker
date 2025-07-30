using System.ComponentModel.DataAnnotations;

namespace PersonalTracker.Web.Models.View_Models;

public class RegisterViewModel
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
    
}