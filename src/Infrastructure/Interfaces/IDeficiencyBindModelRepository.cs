using Domain.Enums;
using Domain.Interfaces;

namespace Infrastructure.Interfaces;
public interface IDeficiencyBindModelRepository<T> : IBaseRepository<T>
{
    public Task<IEnumerable<T>> GetModelsByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType deficiencyType);
}
