using System.Linq.Expressions;

namespace GreenFil.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[]? includes);

    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null
    );

    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>> filter,
        string? includeProperties = null
    );

    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task SaveChangesAsync();
}