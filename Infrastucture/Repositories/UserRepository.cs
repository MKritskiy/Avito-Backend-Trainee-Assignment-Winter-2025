using Domain.Entities;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastucture.Data;
namespace Infrastucture.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) {}


    protected override int? GetId(User entity) => entity.Id;
}
