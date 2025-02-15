using Domain.Entities;
using System.Reflection;

namespace Infrastucture.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<CoinTransfer> CoinTransfers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Item>().HasData(
            new Item { Id = 1, Name = "t-shirt", Price = 80 },
            new Item { Id = 2, Name = "cup", Price = 20 },
            new Item { Id = 3, Name = "book", Price = 50 },
            new Item { Id = 4, Name = "pen", Price = 10 },
            new Item { Id = 5, Name = "powerbank", Price = 200 },
            new Item { Id = 6, Name = "hoody", Price = 300 },
            new Item { Id = 7, Name = "umbrella", Price = 200 },
            new Item { Id = 8, Name = "socks", Price = 10 },
            new Item { Id = 9, Name = "wallet", Price = 50 },
            new Item { Id = 10, Name = "pink-hoody", Price = 500 });
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
