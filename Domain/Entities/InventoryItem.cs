namespace Domain.Entities;

public class InventoryItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public User User { get; set; }
    public Item Item { get; set; }
}
