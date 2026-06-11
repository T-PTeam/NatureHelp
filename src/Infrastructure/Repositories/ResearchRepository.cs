using Domain.Models.Analitycs;
using Domain.Models.Organization;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class ResearchRepository : BaseRepository<Research>, IResearchRepository
{
    public ResearchRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public override async Task<IEnumerable<Research>> GetAllAsync(int scrollCount, IDictionary<string, string?>? filters, User? currentUser = null)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var (sortBy, sortDirection, remainingFilters) = QueryableExtensions.ExtractSorting(filters);
            IQueryable<Research> query = context.Set<Research>()
                .Include(r => r.Laboratory)
                .Include(r => r.Researcher)
                .ApplyFilters(remainingFilters)
                .ApplySorting(sortBy, sortDirection);

            if (scrollCount != -1)
            {
                query = query.Skip(scrollCount * 20).Take(20);
            }

            var list = await query.ToListAsync();

            foreach (var research in list)
            {
                if (research.Laboratory != null) research.Laboratory.Researchers = null;
            }

            return list;
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
