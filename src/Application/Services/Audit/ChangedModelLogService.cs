using Application.Interfaces.Services;
using Application.Interfaces.Services.Audit;
using Domain.Enums;
using Domain.Models.Audit;
using Domain.Models.Nature;
using Infrastructure.Interfaces;
using System.Text.Json;

namespace Application.Services.Audit;
public class ChangedModelLogService : BaseService<ChangedModelLog>, IChangedModelLogService
{
    private readonly IChangedModelLogRepository _changedModelLogRepository;
    public ChangedModelLogService(IChangedModelLogRepository changedModelLogReposiroty)
        : base(changedModelLogReposiroty)
    {
        _changedModelLogRepository = changedModelLogReposiroty;
    }

    public async Task LogDeficiencyChangesAsync<T>(T oldEntity, T newEntity, EDeficiencyType deficiencyType, Guid changedBy) where T : Deficiency
    {
        var changes = new Dictionary<string, object>();

        foreach (var prop in typeof(T).GetProperties())
        {
            var oldValue = prop.GetValue(oldEntity)?.ToString();
            var newValue = prop.GetValue(newEntity)?.ToString();

            if (oldValue != newValue && newValue != null)
            {
                changes[prop.Name] = new { Old = oldValue, New = newValue };
            }
        }

        if (changes.Any())
        {
            var log = new ChangedModelLog
            {
                DeficiencyType = deficiencyType,
                DeficiencyId = newEntity.Id,
                ChangesJson = JsonSerializer.Serialize(changes),
                CreatedBy = changedBy,
                CreatedOn = DateTime.UtcNow
            };

            await _changedModelLogRepository.AddAsync(log);
        }
    }

    public async Task<IEnumerable<ChangedModelLog>> GetChangingHistoryByDeficiencyIdAsync(Guid deficiencyId, EDeficiencyType type)
    {
        return await _changedModelLogRepository.GetListByDeficiencyIdAsync(deficiencyId, type);
    }

}
