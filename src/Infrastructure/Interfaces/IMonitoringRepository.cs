using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Audit;

namespace Infrastructure.Interfaces;
public interface IMonitoringRepository : IBaseRepository<DeficiencyMonitoring>
{
    Task<DeficiencyMonitoring?> GetUserDeficiencyMonitoringAsync(Guid userId, Guid deficiencyId, EDeficiencyType type);
}
