using Domain.Enums;
using Domain.Interfaces;

namespace Application.Interfaces.Services;
public interface IModelByDeficiencyService<T> : IBaseService<T> where T : class
{
    public Task<IEnumerable<T>> GetModelsByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType deficiencyType);
}
