using Application.Models.Dto;
using Domain.Entities;

namespace Application.Models.Responces;

public class InfoResponce
{
    public int Coins { get; set; }
    public List<InventoryItemDto> Inventory { get; set; }
    public CoinHistoryDto CoinHistory { get; set; }
}
