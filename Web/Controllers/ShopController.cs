using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Models.Requests;
using Application.Models.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

namespace Avito_Backend_Trainee_Assignment_Winter_2025.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class ShopController : ControllerBase
{
    private readonly ICoinService _coinService;
    private readonly IUserRepository _userRepository;

    public ShopController(ICoinService coinService, IUserRepository userRepository)
    {
        _coinService = coinService;
        _userRepository = userRepository;
    }


    [HttpGet("info")]
    public async Task<IActionResult> GetUserInfo()
    {
        try
        {
            var userId = await GetCurrentUserId();
            var info = await _coinService.GetInfo(userId);
            return Ok(info);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponce { Errors = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponce { Errors = ex.Message });
        }
    }
    [HttpPost("sendCoin")]
    public async Task<IActionResult> SendCoins([FromBody] SendCoinRequest request)
    {
        try
        {
            int fromUserId = await GetCurrentUserId();
            await _coinService.SendCoin(fromUserId, request.ToUser, request.Amount);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponce { Errors = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponce { Errors = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponce { Errors = ex.Message });
        }
    }

    [HttpGet("buy/{item}")]
    public async Task<IActionResult> BuyItem(string item)
    {
        try
        {
            var userId = await GetCurrentUserId();
            await _coinService.BuyItem(userId, item);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponce { Errors = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponce { Errors = ex.Message });
        }
    }

    private async Task<int> GetCurrentUserId()
    {
        var id = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
        return int.TryParse(id, out int res) ? res : throw new InvalidOperationException("User not found");
    }
}
