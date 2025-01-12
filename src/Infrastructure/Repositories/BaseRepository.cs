using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public BaseRepository(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<T>().ToListAsync();
        }
    }

    public async Task<T> UpdateAsync(T entity)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }

    public async Task<bool> UpdateRangeAsync(IEnumerable<T> list)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            context.Set<T>().UpdateRange(list);
            await context.SaveChangesAsync();
            return true;
        }
    }

    public async Task SaveChangesAsync()
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            await context.SaveChangesAsync();
        }
    }
}
