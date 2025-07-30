using System.Text.Json.Serialization;

namespace PersonalTracker.Web.Models;

public class UserIdDTO
{
    [JsonPropertyName("userId")] 
    public string UserId { get; set; }
}