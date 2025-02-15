using System.Linq.Expressions;

namespace Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<int?> AddAsync(T entity);
    Task<T?> GetByIdAsync(params object[] ids);
    Task<T?> GetByIdAsync(Func<IQueryable<T>, IQueryable<T>>? include = null, params object[] ids);

    Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        int? page = null,
        int? pageSize = null);
    Task<bool> UpdateAsync(T entity);
}
