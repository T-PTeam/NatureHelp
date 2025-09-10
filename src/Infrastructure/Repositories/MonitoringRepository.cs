using Domain.Enums;
using Domain.Models.Audit;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class MonitoringRepository : BaseRepository<DeficiencyMonitoring>, IMonitoringRepository
{
    public MonitoringRepository(IDbContextFactory<ApplicationContext> contextFactory)
        : base(contextFactory) { }

    public async Task<DeficiencyMonitoring?> GetUserDeficiencyMonitoringAsync(Guid userId, Guid deficiencyId, EDeficiencyType type)
    {
        await using var context = _contextFactory.CreateDbContext();

        return await context.Set<DeficiencyMonitoring>()
            .FirstOrDefaultAsync(dm =>
                dm.UserId == userId &&
                dm.DeficiencyId == deficiencyId &&
                dm.DeficiencyType == type);
    }
}
