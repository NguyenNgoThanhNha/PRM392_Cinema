using System.ComponentModel.DataAnnotations;

namespace PRM_API.Common.Payloads.Request;

public class SignUpRequest
{
    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress(ErrorMessage = "Email is invalid format!")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Username is required!")]
    public string Username { get; set; } = null!;
    
    [Required(ErrorMessage = "Password is required!")]
    public string Password { get; set; } = null!;

}