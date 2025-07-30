using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PersonalTracker.Web.Models.View_Models;

public class LoginViewModel
{
    [Required(ErrorMessage ="Username is required")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must have at least 6 characters")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$",  ErrorMessage = "Password must contain at least one uppercase letter and one digit.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}