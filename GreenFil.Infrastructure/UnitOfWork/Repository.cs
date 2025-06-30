using System.Linq.Expressions;
using GreenFil.Application.Interfaces;
using GreenFil.Infrastructure.GreenFilContext;
using Microsoft.EntityFrameworkCore;

namespace GreenFil.Infrastructure.UnitOfWork;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly GreenFilContext.GreenfilContext _context;
    private readonly DbSet<T> _entities;

    public Repository(GreenFilContext.GreenfilContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id) => await _entities.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _entities;

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = _entities;

        if (filter != null)
            query = query.Where(filter);

        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp.Trim());
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = _entities.Where(filter);

        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp.Trim());
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task AddAsync(T entity) => await _entities.AddAsync(entity);

    public void Update(T entity) => _entities.Update(entity);

    public void Remove(T entity) => _entities.Remove(entity);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}