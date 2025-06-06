﻿using Domain.Interfaces;
using Domain.Models.Audit;
using Domain.Models.Nature;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

public class DeficiencyRepository<D> : BaseRepository<D> where D : Deficiency
{
    private readonly IBaseRepository<ChangedModelLog> _changedModelLogRepository;

    public DeficiencyRepository(
        IDbContextFactory<ApplicationContext> contextFactory,
        IBaseRepository<ChangedModelLog> changedModelLogRepository)
        : base(contextFactory)
    {
        _changedModelLogRepository = changedModelLogRepository;
    }

    public override async Task<IEnumerable<D>> GetAllAsync(int scrollCount, IDictionary<string, string?>? filters)
    {
        var context = _contextFactory.CreateDbContext();

        var query = context.Set<D>()
            .Include(d => d.Creator)
            .Include(d => d.ResponsibleUser)
            .Include(d => d.Location)
            .ApplyFilters(filters ?? new Dictionary<string, string?>());

        if (scrollCount != -1)
        {
            query = query.Skip(scrollCount * 20).Take(20);
        }

        return await query.ToListAsync();
    }

    public override async Task<D?> GetByIdAsync(Guid id)
    {
        var context = _contextFactory.CreateDbContext();

        return await context.Set<D>()
            .Include(d => d.Creator)
                .ThenInclude(c => c.Address)
            .Include(d => d.ResponsibleUser)
                .ThenInclude(u => u.Address)
            .Include(d => d.Location)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<D> UpdateAsync(D entity)
    {
        var context = _contextFactory.CreateDbContext();

        context.Set<D>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}
