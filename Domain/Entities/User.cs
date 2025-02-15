using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public int Coins { get; set; } = 1000;
    public ICollection<InventoryItem> InventoryItems { get; }
    [InverseProperty("FromUser")]
    public ICollection<CoinTransfer> SentCoinTransfers { get; }
    [InverseProperty("ToUser")]
    public ICollection<CoinTransfer> ReceivedCoinTransfers { get; }
}
