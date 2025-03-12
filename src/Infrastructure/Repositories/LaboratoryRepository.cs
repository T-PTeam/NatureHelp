using Domain.Models.Organization;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class LaboratoryRepository : BaseRepository<Laboratory>, ILaboratoryRepository
{
    public LaboratoryRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public override async Task<IEnumerable<Laboratory>> GetAllAsync()
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<Laboratory>()
                .Include(l => l.Location)
                .Select(l => new Laboratory
                {
                    Id = l.Id,
                    Title = l.Title,
                    Location = l.Location,

                    ResearchersCount = l.Researchers != null ? l.Researchers.Count : 0,
                })
                .ToListAsync();
        }
    }

    public override async Task<Laboratory?> GetByIdAsync(Guid id)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<Laboratory>()
                .Include(l => l.Location)
                .Select(l => new Laboratory
                {
                    Id = l.Id,
                    Title = l.Title,
                    Location = l.Location,

                    ResearchersCount = l.Researchers != null ? l.Researchers.Count : 0,
                })
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
