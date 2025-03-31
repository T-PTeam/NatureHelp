using Domain.Models.Nature;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Repositories;
public class DeficiencyRepository<D> : BaseRepository<D> where D : Deficiency
{
    public DeficiencyRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public override async Task<IEnumerable<D>> GetAllAsync(int scrollCount)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<D>()
                .Include(d => d.Creator)
                .Include(d => d.ResponsibleUser)
                .Include(d => d.Location)
                .Skip(scrollCount * 20).Take(20)
                .ToListAsync();
        }
    }

    public override async Task<D?> GetByIdAsync(Guid id)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<D>()
                .Include(d => d.Creator)
                    .ThenInclude(c => c.Address)
                .Include(d => d.ResponsibleUser)
                    .ThenInclude(u => u.Address)
                .Include(d => d.Location)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }

    public override async Task<D> UpdateAsync(D entity)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            context.Set<D>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
