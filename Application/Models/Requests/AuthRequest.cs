using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

public class AuthRequest
{
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
