using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastucture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Repositories;


public class InventoryItemRepository : BaseRepository<InventoryItem>, IInventoryItemRepository
{
    public InventoryItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    protected override int? GetId(InventoryItem entity) => entity.Id;
}
