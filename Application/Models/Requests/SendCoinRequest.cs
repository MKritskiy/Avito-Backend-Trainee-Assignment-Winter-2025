using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

public class SendCoinRequest
{
    [Required]
    public string ToUser { get; set; } = null!;
    [Required]
    public int Amount { get; set; }
}
