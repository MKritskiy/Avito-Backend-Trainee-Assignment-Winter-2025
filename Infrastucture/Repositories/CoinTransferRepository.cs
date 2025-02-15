using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastucture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Repositories;


public class CoinTransferRepository : BaseRepository<CoinTransfer>, ICoinTransferRepository
{
    public CoinTransferRepository(ApplicationDbContext context) : base(context)
    {
    }


    protected override int? GetId(CoinTransfer entity) => entity.Id;

}
