using Application.Interfaces.Services;
using Application.Models.Requests;
using Application.Models.Responces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace Avito_Backend_Trainee_Assignment_Winter_2025.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
    {
        try
        {
            var responce = await _authService.Authenticate(request);
            return Ok(responce);
        }
        catch (InvalidCredentialException)
        {
            return Unauthorized(new ErrorResponce { Errors = "Invalid credentials" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponce { Errors = ex.Message });
        }
    }
}
