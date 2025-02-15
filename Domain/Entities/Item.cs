using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Item
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Price { get; set; }
}
