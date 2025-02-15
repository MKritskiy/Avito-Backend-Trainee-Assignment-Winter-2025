using Application.Models.Requests;
using Application.Models.Responces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
namespace Tests;
public class ShopControllerIntegrationTests : BaseTest, IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ShopControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    
    [Fact]
    public async Task BuyItem_ReturnsOk_WhenItemIsPurchased()
    {
        // Arrange: получаем токен для авторизации
        string token = await GetAuthTokenAsync("username");
        SetAuthorizationHeader(token);

        string item = "t-shirt";  // Пример мерча

        // Act
        var response = await _client.GetAsync($"/api/buy/{item}");

        // Assert
        response.EnsureSuccessStatusCode(); // Проверяем, что статус ответа успешен
    }

    [Fact]
    public async Task SendCoins_ReturnsOk_WhenCoinsAreSent()
    {
        // Arrange
        string token = await GetAuthTokenAsync("user1");
        SetAuthorizationHeader(token);
        await GetAuthTokenAsync("user2");


        var sendCoinRequest = new SendCoinRequest
        {
            ToUser = "user2",
            Amount = 100
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(sendCoinRequest),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/sendCoin", content);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Authenticate_ReturnsUnauthorized_WhenInvalidCredentials()
    {
        // Arrange
        var authRequest1 = new AuthRequest
        {
            Username = "validUsername",
            Password = "validPassword"
        };
        var authRequest2 = new AuthRequest
        {
            Username = "validUsername",
            Password = "invalidPassword"
        };

        var content1 = new StringContent(
            JsonConvert.SerializeObject(authRequest1),
            Encoding.UTF8,
            "application/json"
        );
        var content2 = new StringContent(
            JsonConvert.SerializeObject(authRequest2),
            Encoding.UTF8,
            "application/json"
        );
        // Act
        var response1 = await _client.PostAsync("/api/auth", content1);
        var response2 = await _client.PostAsync("/api/auth", content2);


        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response2.StatusCode);
    }


    [Fact]
    public async Task SendCoins_ReturnsBadRequest_WhenInvalidAmount()
    {
        // Arrange
        string token = await GetAuthTokenAsync("username");
        SetAuthorizationHeader(token);

        var sendCoinRequest = new SendCoinRequest
        {
            ToUser = "validUsername",
            Amount = -10 // Некорректная сумма
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(sendCoinRequest),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/sendCoin", content);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}