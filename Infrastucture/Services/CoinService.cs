using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Models.Dto;
using Application.Models.Requests;
using Application.Models.Responces;
using Domain.Entities;
using Infrastucture.General;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Services;

public class CoinService : ICoinService
{
    private readonly ICoinTransferRepository _coinTransferRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IInventoryItemRepository _inventoryItemRepository;
    private readonly IUserRepository _userRepository;

    public CoinService(ICoinTransferRepository coinTransferRepository, IItemRepository itemRepository, IInventoryItemRepository inventoryItemRepository, IUserRepository userRepository)
    {
        _coinTransferRepository = coinTransferRepository;
        _itemRepository = itemRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _userRepository = userRepository;
    }

    public async Task BuyItem(int userId, string ItemName)
    {
        using var transaction = Helpers.CreateTransactionScope();
        try
        {
            Item? item = (await _itemRepository.Get(i => i.Name == ItemName)).FirstOrDefault();
            User? user = await _userRepository.GetByIdAsync(userId);
            if (user == null || item == null) throw new InvalidOperationException();

            if (user.Coins < item.Price) throw new InvalidOperationException();

            user.Coins -= item.Price;


            await _inventoryItemRepository.AddAsync(new InventoryItem()
            {
                UserId = userId,
                ItemId = item.Id,
            });
            await _userRepository.UpdateAsync(user);
            transaction.Complete();
        }
        catch
        {
            throw;
        }
    }

    public async Task<InfoResponce> GetInfo(int userId)
    {
        User? user = await _userRepository.GetByIdAsync(ids: userId,
            include: q => q
                .Include(u => u.InventoryItems)
                    .ThenInclude(ii => ii.Item)
                .Include(u => u.SentCoinTransfers)
                    .ThenInclude(ct=> ct.ToUser)
                .Include(u => u.ReceivedCoinTransfers)
                    .ThenInclude(ct => ct.FromUser)
            );


        if (user == null) throw new InvalidOperationException();


        var inventoryGroups = user.InventoryItems
        .GroupBy(ii => ii.Item.Name);

        var inventoryDtos = inventoryGroups.Select(g => new InventoryItemDto
        {
            Type = g.Key,
            Quantity = g.Count()
        }).ToList();


        var receivedTransfers = user.ReceivedCoinTransfers
            .Select(ct => new RecievedTransferDto
            {
                FromUser = ct.FromUser?.Username ?? "System",
                Amount = ct.Amount,
            }).ToList();

        var sentTransfers = user.SentCoinTransfers
            .Select(ct => new SentTransferDto
            {
                ToUser = ct.ToUser?.Username ?? "System",
                Amount = ct.Amount
            }).ToList();

        return new InfoResponce
        {
            Coins = user.Coins,
            Inventory = inventoryDtos,
            CoinHistory = new CoinHistoryDto
            {
                Recieved = receivedTransfers,
                Sent = sentTransfers
            }
        };
    }

    public async Task SendCoin(int fromUserId, string toUsername, int amount)
    {

        if (amount<=0) throw new ArgumentException();
        using var transaction = Helpers.CreateTransactionScope();
        try
        {
            var fromUser = await _userRepository.GetByIdAsync(fromUserId);
            var toUser = (await _userRepository.Get(u => u.Username == toUsername)).FirstOrDefault();
            if (fromUser == null || toUser == null)
                throw new InvalidOperationException("User not found");

            if (fromUser.Coins < amount || amount<0)
                throw new InvalidOperationException("Insufficient funds");

            fromUser.Coins -= amount;
            toUser.Coins += amount;

            CoinTransfer transfer = new CoinTransfer()
            {
                Amount = amount,
                FromUserId = fromUser.Id,
                ToUserId = toUser.Id
            };

            await _userRepository.UpdateAsync(toUser);
            await _userRepository.UpdateAsync(fromUser);
            await _coinTransferRepository.AddAsync(transfer);
            transaction.Complete();
        }
        catch 
        {
            throw;
        }

    }
}
