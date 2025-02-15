using Application.Models.Responces;
using Application.Models.Requests;
using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IAuthService
{
    Task<int> CreateUser(User user);
    Task<AuthResponce> Authenticate(AuthRequest auth);
}
