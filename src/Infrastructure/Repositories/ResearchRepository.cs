using Domain.Models.Analitycs;
using Domain.Models.Organization;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class ResearchRepository : BaseRepository<Research>, IResearchRepository
{
    public ResearchRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public async Task<IEnumerable<Research>> GetByLabId(Guid labId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<Research>().Where(u => u.LaboratoryId == labId).ToArrayAsync();
        }
    }
}
