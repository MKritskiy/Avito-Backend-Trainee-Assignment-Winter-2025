using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Models.Requests;
using Moq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

public class BaseTest
{
    protected readonly Mock<IEncrypt> MockEncrypt;
    protected readonly Mock<ITokenService> MockTokenService;
    protected readonly Mock<IUserRepository> MockUserRepository;
    protected readonly Mock<ICoinTransferRepository> MockCoinTransferRepository;
    protected readonly Mock<IItemRepository> MockItemRepository;
    protected readonly Mock<IInventoryItemRepository> MockInventoryItemRepository;
    protected readonly Mock<ICoinService> MockCoinService;
    protected readonly Mock<IAuthService> MockAuthService;
    protected HttpClient _client;

    public BaseTest()
    {
        MockEncrypt = new Mock<IEncrypt>();
        MockTokenService = new Mock<ITokenService>();
        MockUserRepository = new Mock<IUserRepository>();
        MockCoinTransferRepository = new Mock<ICoinTransferRepository>();
        MockItemRepository = new Mock<IItemRepository>();
        MockInventoryItemRepository = new Mock<IInventoryItemRepository>();
        MockCoinService = new Mock<ICoinService>();
        MockAuthService = new Mock<IAuthService>();
    }
    protected async Task<string> GetAuthTokenAsync(string username)
    {
        var authRequest = new AuthRequest
        {
            Username = username, // Замените на действительные данные
            Password = "validPassword"  // Замените на действительные данные
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(authRequest),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/api/auth", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            // Предполагается, что ответ содержит только токен, если он успешен
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent)["token"];
            return token;
        }

        throw new Exception("Authentication failed");
    }

    protected void SetAuthorizationHeader(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

}
