using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
{
    protected readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public BaseRepository(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    private ApplicationContext GetContext() => _contextFactory.CreateDbContext();

    public virtual async Task<IEnumerable<T>> GetAllAsync(int scrollCount, IDictionary<string, string?>? filters = null)
    {
        var context = GetContext();

        var query = context.Set<T>().AsQueryable().ApplyFilters(filters ?? new Dictionary<string, string?>());

        if (scrollCount != -1)
        {
            query = query.Skip(scrollCount * 20).Take(20);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        var context = GetContext();
        return await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var context = GetContext();
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        var context = GetContext();
        await context.Set<T>().AddRangeAsync(entities);
        await context.SaveChangesAsync();
        return entities;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        var context = GetContext();
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> UpdateRangeAsync(IEnumerable<T> list)
    {
        var context = GetContext();
        context.Set<T>().UpdateRange(list);
        await context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<int> GetTotalCount()
    {
        var context = GetContext();
        return await context.Set<T>().CountAsync();
    }

    public async Task SaveChangesAsync()
    {
        var context = GetContext();
        await context.SaveChangesAsync();
    }
}
