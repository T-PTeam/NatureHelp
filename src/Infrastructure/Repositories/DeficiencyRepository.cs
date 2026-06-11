using Domain.Interfaces;
using Domain.Models.Audit;
using Domain.Models.Nature;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

public class DeficiencyRepository<D, MAPT> : BaseRepository<D>, IMapObjectsRepository<D, MAPT> where D : Deficiency where MAPT : DeficiencyMapDto
{
    private readonly IBaseRepository<ChangedModelLog> _changedModelLogRepository;

    public DeficiencyRepository(
        IDbContextFactory<ApplicationContext> contextFactory,
        IBaseRepository<ChangedModelLog> changedModelLogRepository)
        : base(contextFactory)
    {
        _changedModelLogRepository = changedModelLogRepository;
    }

    public async Task<IEnumerable<MAPT>> GetMapObjects(IDictionary<string, string?>? filters, Domain.Models.Organization.User? currentUser = null)
    {
        using var context = _contextFactory.CreateDbContext();

        IQueryable<DeficiencyMapDto> result;

        if (typeof(D) == typeof(WaterDeficiency))
        {
            result = context.WaterDeficiencies
                .Where(d => d.IsPublic || (currentUser != null && d.Creator.OrganizationId == currentUser.OrganizationId))
                .AsNoTracking()
                .Select(d => new DeficiencyMapDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    Type = d.Type,
                    EDangerState = d.EDangerState,
                    Longitude = d.Longitude,
                    Latitude = d.Latitude,
                    RadiusAffected = d.RadiusAffected,
                    CreatorFullName = d.Creator != null ? $"{d.Creator.FirstName} {d.Creator.LastName}" : string.Empty,
                    ResponsibleUserFullName = d.ResponsibleUser != null ? $"{d.ResponsibleUser.FirstName} {d.ResponsibleUser.LastName}" : string.Empty
                });
        }
        else
        {
            result = context.SoilDeficiencies
                .Where(d => d.IsPublic || (currentUser != null && d.Creator.OrganizationId == currentUser.OrganizationId))
                .AsNoTracking()
                .Select(d => new DeficiencyMapDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    Type = d.Type,
                    EDangerState = d.EDangerState,
                    Longitude = d.Longitude,
                    Latitude = d.Latitude,
                    RadiusAffected = d.RadiusAffected,
                    CreatorFullName = d.Creator != null ? $"{d.Creator.FirstName} {d.Creator.LastName}" : string.Empty,
                    ResponsibleUserFullName = d.ResponsibleUser != null ? $"{d.ResponsibleUser.FirstName} {d.ResponsibleUser.LastName}" : string.Empty
                });
        }

        return await result
            .ApplyFilters(filters)
            .Cast<MAPT>()
            .ToListAsync();
    }

    public async Task<IEnumerable<D>> GetAllAsync(int scrollCount, IDictionary<string, string?>? filters, Domain.Models.Organization.User? currentUser)
    {
        var context = _contextFactory.CreateDbContext();

        var (sortBy, sortDirection, remainingFilters) = QueryableExtensions.ExtractSorting(filters);
        var query = context.Set<D>()
            .Where(d => d.IsPublic || (currentUser != null && d.Creator.OrganizationId == currentUser.OrganizationId))
            .Include(d => d.Creator)
            .Include(d => d.ResponsibleUser)
            .ApplyFilters(remainingFilters)
            .ApplySorting(sortBy, sortDirection);

        if (scrollCount != -1)
        {
            query = query.Skip(scrollCount * 20).Take(20);
        }

        return await query.ToListAsync();
    }

    public override async Task<D?> GetByIdAsync(Guid id)
    {
        var context = _contextFactory.CreateDbContext();

        var entity = await context.Set<D>()
            .Include(d => d.Creator)
            .Include(d => d.ResponsibleUser)
            .Include(d => d.DeficiencyMonitoring)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        return entity;
    }

    public override async Task<D> UpdateAsync(D entity)
    {
        var context = _contextFactory.CreateDbContext();

        context.Set<D>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}
