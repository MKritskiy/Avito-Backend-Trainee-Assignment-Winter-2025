using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class CoinTransfer
{
    public int Id { get; set; }
    public int Amount { get; set; }
    [ForeignKey("FromUser")]
    public int? FromUserId { get; set; }
    public User? FromUser { get; set; }
    [ForeignKey("ToUser")]
    public int? ToUserId { get; set; }
    public User? ToUser { get; set; }
}
