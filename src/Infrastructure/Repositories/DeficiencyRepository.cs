using Domain.Models.Nature;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class DeficiencyRepository<D> : BaseRepository<D>, IDeficiencyRepository<D> where D : Deficiency
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
                .Include(d => d.ResponsibleUser)
                .Include(d => d.Location)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
