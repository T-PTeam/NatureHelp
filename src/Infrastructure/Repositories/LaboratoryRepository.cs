using Domain.Interfaces;
using Domain.Models.Organization;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class LaboratoryRepository : BaseRepository<Laboratory>, IMapObjectsRepository<Laboratory, LaboratoryMapDto>
{
    public LaboratoryRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public async Task<IEnumerable<LaboratoryMapDto>> GetMapObjects(IDictionary<string, string?>? filters, User? currentUser = null)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var query = context.Laboratories
                .Where(l => l.IsPublic || (currentUser != null && l.CreatedBy == currentUser.Id))
                .AsNoTracking()
                .Select(l => new LaboratoryMapDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Longitude = l.Longitude,
                    Latitude = l.Latitude,
                    ResearchersCount = l.Researchers != null ? l.Researchers.Count : 0,
                    Researchers = l.Researchers != null
                        ? l.Researchers
                            .Select(r => new ResearcherDto
                            {
                                Id = r.Id,
                                FullName = $"{r.FirstName} {r.LastName}"
                            }).ToList()
                        : new List<ResearcherDto>()
                });

            if (filters != null && filters.Any())
            {
                query = query.ApplyFilters(filters);
            }

            return await query.ToListAsync();
        }
    }

    public override async Task<IEnumerable<Laboratory>> GetAllAsync(int scrollCount, IDictionary<string, string?>? filters, User? currentUser = null)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var query = context.Set<Laboratory>()
                .Where(l => l.IsPublic || (currentUser != null && l.CreatedBy == currentUser.Id))
                .Include(l => l.Researchers)
                .Select(l => new Laboratory
                {
                    Id = l.Id,
                    Title = l.Title,
                    Researchers = l.Researchers != null
                        ? l.Researchers.Select(r => new User()
                        {
                            FirstName = r.FirstName,
                            LastName = r.LastName
                        }).ToList()
                        : new List<User>(),
                    ResearchersCount = l.Researchers != null ? l.Researchers.Count : 0,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude
                });

            if (filters != null && filters.Any())
            {
                query = query.ApplyFilters(filters);
            }

            if (scrollCount != -1)
            {
                query = query.Skip(scrollCount * 20).Take(20);
            }

            return await query.ToListAsync();
        }
    }

    public override async Task<Laboratory?> GetByIdAsync(Guid id)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<Laboratory>()
                .Include(l => l.Researchers)
                .Select(l => new Laboratory
                {
                    Id = l.Id,
                    Title = l.Title,
                    Researchers = l.Researchers,
                    ResearchersCount = l.Researchers != null ? l.Researchers.Count : 0,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude
                })
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
