using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using System.Linq.Expressions;

namespace Infrastucture.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class 
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<int?> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return GetId(entity);
    }
    public virtual async Task<T?> GetByIdAsync(params object[] ids)
    {
        return await _dbSet.FindAsync(ids);
    }

    public virtual async Task<T?> GetByIdAsync(Func<IQueryable<T>, IQueryable<T>>? include = null, params object[] ids)
    {
        IQueryable<T> query = _dbSet;

        if (include!=null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(e => ids.Contains(EF.Property<object>(e, "Id")));
    }

    public virtual async Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            int? page = null,
            int? pageSize = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }

        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return await query.ToListAsync();

    }
    public async Task<bool> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SaveChangesWithRetryAsync(DbContext context)
    {
        int maxRetries = 5;
        var policy = Policy.Handle<PostgresException>(ex => ex.SqlState == "40001") // ошибка из-за конкуренции
            .WaitAndRetryAsync(maxRetries, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))); // Экспоненциальная задержка

        try
        {
            // Применяем политику повторных попыток
            await policy.ExecuteAsync(async () =>
            {
                await context.SaveChangesAsync();
            });
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
            return false;
        }
    }
    protected abstract int? GetId(T entity);
}
