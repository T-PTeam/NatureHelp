using Domain.Models.Analitycs;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class ResearchRepository : BaseRepository<Research>, IResearchRepository
{
    public ResearchRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public override async Task<IEnumerable<Research>> GetAllAsync(int scrollCount, IDictionary<string, string?>? filters)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            if (scrollCount == -1)
            {
                IQueryable<Research> fullList = context.Set<Research>()
                    .Include(r => r.Laboratory)
                        .ThenInclude(l => l.Location)
                    .Include(r => r.Researcher);

                foreach (var research in fullList)
                {
                    if (research.Laboratory != null) research.Laboratory.Researchers = null;
                }

                return await fullList.ToListAsync();
            }

            IQueryable<Research> list = context.Set<Research>()
                    .Include(r => r.Laboratory)
                        .ThenInclude(l => l.Location)
                    .Include(r => r.Researcher)
                    .Skip(scrollCount * 20).Take(20);

            foreach (var research in list)
            {
                if (research.Laboratory != null) research.Laboratory.Researchers = null;
            }

            return await list.ToListAsync();
        }
    }

    public async Task<IEnumerable<Research>> GetByLabId(Guid labId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<Research>().Where(u => u.LaboratoryId == labId).ToArrayAsync();
        }
    }
}
