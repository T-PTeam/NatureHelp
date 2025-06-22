using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Audit;
using Domain.Models.Nature;

namespace Application.Interfaces.Services.Audit;
public interface IChangedModelLogService : IBaseService<ChangedModelLog>
{
    public Task LogDeficiencyChangesAsync<T>(T oldEntity, T newEntity, EDeficiencyType deficiencyType, Guid changedBy) where T : Deficiency;
    public Task LogDeficiencyAttachmentChangeAsync<T>(Guid deficiencyId, string fileName, long fileSize, string previewUrl, EDeficiencyType deficiencyType) where T : Deficiency;
    public Task<IEnumerable<ChangedModelLog>> GetChangingHistoryByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType type);
}
