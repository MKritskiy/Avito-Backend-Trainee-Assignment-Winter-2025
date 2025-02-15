using Application.Interfaces.Services;
using Domain.Entities;
using Infrastucture.Services;
using Moq;
using System.Linq.Expressions;

namespace Tests;

public class CoinServiceTests : BaseTest
{
    private readonly ICoinService _coinService;
    public CoinServiceTests()
    {
        _coinService = new CoinService(MockCoinTransferRepository.Object, MockItemRepository.Object, MockInventoryItemRepository.Object, MockUserRepository.Object);
    }

    [Fact]
    public async Task BuyItem_ShouldDeductCoins_WhenUserHasEnoughCoins()
    {
        // Arrange
        var userId = 1;
        var itemName = "item1";
        var item = new Item { Id = 1, Name = itemName, Price = 100 };
        var user = new User { Id = userId, Coins = 200 };

        MockItemRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Item, bool>>>(), null, null, null, null))
                           .ReturnsAsync(new List<Item> { item });

        MockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                           .ReturnsAsync(user);

        MockInventoryItemRepository.Setup(repo => repo.AddAsync(It.IsAny<InventoryItem>()))
                                     .ReturnsAsync(1);

        // Act
        await _coinService.BuyItem(userId, itemName);

        // Assert
        Assert.Equal(100, user.Coins);
        MockInventoryItemRepository.Verify(repo => repo.AddAsync(It.Is<InventoryItem>(ii => ii.UserId == userId && ii.ItemId == item.Id)), Times.Once);
        MockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task BuyItem_ShouldThrowException_WhenUserHasNotEnoughCoins()
    {
        // Arrange
        var userId = 1;
        var itemName = "item1";
        var item = new Item { Id = 1, Name = itemName, Price = 100 };
        var user = new User { Id = userId, Coins = 50 };

        MockItemRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Item, bool>>>(), null, null, null, null))
                           .ReturnsAsync(new List<Item> { item });

        MockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                           .ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _coinService.BuyItem(userId, itemName));
    }
    [Fact]
    public async Task SendCoin_ShouldTransferCoins_WhenUsersExistAndHaveSufficientCoins()
    {
        // Arrange
        var fromUserId = 1;
        var toUsername = "user2";
        var amount = 50;
        var fromUser = new User { Id = fromUserId, Coins = 100 };
        var toUser = new User { Id = 2, Coins = 30 };

        MockUserRepository.Setup(repo => repo.GetByIdAsync(fromUserId))
                           .ReturnsAsync(fromUser);
        MockUserRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), null, null, null, null))
                           .ReturnsAsync(new List<User> { toUser });

        // Act
        await _coinService.SendCoin(fromUserId, toUsername, amount);

        // Assert
        Assert.Equal(50, fromUser.Coins);  // 100 - 50
        Assert.Equal(80, toUser.Coins);    // 30 + 50
        MockUserRepository.Verify(repo => repo.UpdateAsync(fromUser), Times.Once);
        MockUserRepository.Verify(repo => repo.UpdateAsync(toUser), Times.Once);
    }
}
