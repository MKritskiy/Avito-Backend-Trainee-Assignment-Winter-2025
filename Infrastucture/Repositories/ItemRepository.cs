using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastucture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Repositories;

public class ItemRepository : BaseRepository<Item>, IItemRepository
{
    public ItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    protected override int? GetId(Item entity) => entity.Id;
}
