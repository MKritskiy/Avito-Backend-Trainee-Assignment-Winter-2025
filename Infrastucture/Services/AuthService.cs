using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Models.Requests;
using Application.Models.Responces;
using Domain.Entities;
using Infrastucture.Repositories;
using System.Security.Authentication;

namespace Infrastucture.Services;

public class AuthService : IAuthService
{
    private readonly IEncrypt _encrypt;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AuthService(IEncrypt encrypt, ITokenService tokenService, IUserRepository userRepository)
    {
        _encrypt = encrypt;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<AuthResponce> Authenticate(AuthRequest auth)
    {
        User? user = (await _userRepository.Get(u => u.Username == auth.Username)).FirstOrDefault();
        if (user == null)
        {
            user = new User() { Username = auth.Username, PasswordHash = auth.Password };
            int id = await CreateUser(user);
        } 
        else if (user.PasswordHash != _encrypt.HashPassword(auth.Password, user.PasswordSalt))
        {
            throw new InvalidCredentialException();
        }

        var token = _tokenService.GenerateToken(user);
        return new AuthResponce() { Token = token };
    }

    public async Task<int> CreateUser(User user)
    {
        if (user.Username == null || user.PasswordHash == null) throw new InvalidOperationException("Incorrect User Data");
        user.PasswordSalt = General.Helpers.GenerateSalt();
        user.PasswordHash = _encrypt.HashPassword(user.PasswordHash, user.PasswordSalt);
        return await _userRepository.AddAsync(user) ?? 0;
    }
}
