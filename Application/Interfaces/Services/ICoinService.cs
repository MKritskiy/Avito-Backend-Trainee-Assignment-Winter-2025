using Application.Models.Responces;
using Application.Models.Requests;

namespace Application.Interfaces.Services;

public interface ICoinService
{
    Task<InfoResponce> GetInfo(int userId);
    Task SendCoin(int fromUserId, string toUsername, int amount);
    Task BuyItem(int userId, string ItemName);
}
