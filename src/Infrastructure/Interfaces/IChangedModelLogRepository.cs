using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Audit;

namespace Infrastructure.Interfaces;
public interface IChangedModelLogRepository : IBaseRepository<ChangedModelLog>
{
    public Task<IEnumerable<ChangedModelLog>> GetListByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType type);
}
