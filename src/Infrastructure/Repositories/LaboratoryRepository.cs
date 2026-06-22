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

    private static IQueryable<Laboratory> ApplyVisibilityFilter(IQueryable<Laboratory> query, User? currentUser)
    {
        var laboratoryIds = currentUser?.UserLaboratories?
            .Select(ul => ul.LaboratoryId)
            .Distinct()
            .ToList() ?? new List<Guid>();

        if (currentUser?.LaboratoryId is Guid primaryLaboratoryId)
        {
            laboratoryIds.Add(primaryLaboratoryId);
        }

        if (currentUser == null)
        {
            return query.Where(l => l.IsPublic);
        }

        return query.Where(l =>
            l.IsPublic
            || l.CreatedBy == currentUser.Id
            || laboratoryIds.Contains(l.Id));
    }

    public async Task<IEnumerable<LaboratoryMapDto>> GetMapObjects(IDictionary<string, string?>? filters, User? currentUser = null)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var query = ApplyVisibilityFilter(context.Laboratories.AsNoTracking(), currentUser)
                .Select(l => new LaboratoryMapDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Longitude = l.Longitude,
                    Latitude = l.Latitude,
                    ResearchersCount = l.UserLaboratories != null ? l.UserLaboratories.Count : 0,
                    Researchers = l.UserLaboratories != null
                        ? l.UserLaboratories
                            .Select(r => new ResearcherDto
                            {
                                Id = r.User.Id,
                                FullName = $"{r.User.FirstName} {r.User.LastName}"
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
            var (sortBy, sortDirection, remainingFilters) = QueryableExtensions.ExtractSorting(filters);
            var query = ApplyVisibilityFilter(context.Set<Laboratory>(), currentUser)
                .ApplyFilters(remainingFilters)
                .ApplySorting(sortBy, sortDirection)
                .Select(l => new Laboratory
                {
                    Id = l.Id,
                    Title = l.Title,
                    CreatedBy = l.CreatedBy,
                    CreatedOn = l.CreatedOn,
                    Researchers = l.UserLaboratories != null
                        ? l.UserLaboratories.Select(r => new User()
                        {
                            Id = r.User.Id,
                            FirstName = r.User.FirstName,
                            LastName = r.User.LastName
                        }).ToList()
                        : new List<User>(),
                    ResearcherIds = l.UserLaboratories != null
                        ? l.UserLaboratories.Select(r => r.UserId).ToList()
                        : new List<Guid>(),
                    ResearchersCount = l.UserLaboratories != null ? l.UserLaboratories.Count : 0,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    Address = l.Address,
                    IsPublic = l.IsPublic,
                });

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
                .Select(l => new Laboratory
                {
                    Id = l.Id,
                    Title = l.Title,
                    CreatedBy = l.CreatedBy,
                    CreatedOn = l.CreatedOn,
                    Researchers = l.UserLaboratories != null
                        ? l.UserLaboratories.Select(r => new User
                        {
                            Id = r.User.Id,
                            FirstName = r.User.FirstName,
                            LastName = r.User.LastName,
                            Email = r.User.Email,
                            OrganizationId = r.User.OrganizationId,
                            LaboratoryId = r.User.LaboratoryId,
                        }).ToList()
                        : new List<User>(),
                    ResearcherIds = l.UserLaboratories != null
                        ? l.UserLaboratories.Select(r => r.UserId).ToList()
                        : new List<Guid>(),
                    ResearchersCount = l.UserLaboratories != null ? l.UserLaboratories.Count : 0,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    Address = l.Address,
                    IsPublic = l.IsPublic
                })
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
