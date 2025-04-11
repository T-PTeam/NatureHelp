using Domain.Models.Organization;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class LaboratoryRepository : BaseRepository<Laboratory>
{
    public LaboratoryRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public override async Task<IEnumerable<Laboratory>> GetAllAsync(int scrollCount)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            if (scrollCount == -1)
            {
                return await context.Set<Laboratory>()
                    .Include(l => l.Location)
                    .Include(l => l.Researchers)
                    .Select(l => new Laboratory
                    {
                        Id = l.Id,
                        Title = l.Title,
                        Location = l.Location,
                        Researchers = l.Researchers != null
                            ? l.Researchers.Select(r => new User()
                            {
                                FirstName = r.FirstName,
                                LastName = r.LastName
                            }).ToList()
                            : new List<User>(),
                        ResearchersCount = l.Researchers != null ? l.Researchers.Count : 0,
                    })
                    .ToListAsync();
            }

            return await context.Set<Laboratory>()
                .Include(l => l.Location)
                .Include(l => l.Researchers)
                .Select(l => new Laboratory
                {
                    Id = l.Id,
                    Title = l.Title,
                    Location = l.Location,
                    Researchers = l.Researchers != null
                        ? l.Researchers.Select(r => new User()
                        {
                            FirstName = r.FirstName,
                            LastName = r.LastName
                        }).ToList()
                        : new List<User>(),
                    ResearchersCount = l.Researchers != null ? l.Researchers.Count : 0,
                })
                .Skip(scrollCount * 20).Take(20)
                .ToListAsync();
        }
    }

    public override async Task<Laboratory?> GetByIdAsync(Guid id)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<Laboratory>()
                .Include(l => l.Location)
                .Include(l => l.Researchers)
                .Select(l => new Laboratory
                {
                    Id = l.Id,
                    Title = l.Title,
                    Location = l.Location,
                    Researchers = l.Researchers,
                    ResearchersCount = l.Researchers != null ? l.Researchers.Count : 0,
                })
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
