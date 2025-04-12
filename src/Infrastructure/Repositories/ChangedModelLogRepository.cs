using Domain.Enums;
using Domain.Models.Audit;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class ChangedModelLogRepository : BaseRepository<ChangedModelLog>, IChangedModelLogRepository
{
    public ChangedModelLogRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public async Task<IEnumerable<ChangedModelLog>> GetListByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType type)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Set<ChangedModelLog>()
                .Where(e => e.DeficiencyId == deficiencyId && e.DeficiencyType == type)
                .OrderByDescending(e => e.CreatedOn)
                .ToListAsync();
        }
    }
}
