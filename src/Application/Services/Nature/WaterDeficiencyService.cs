using Application.Interfaces.Services;
using Application.Interfaces.Services.Audit;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Nature;

namespace Application.Services.Nature;
public class WaterDeficiencyService : BaseService<WaterDeficiency>
{
    private readonly IChangedModelLogService _logService;
    public WaterDeficiencyService(
        IBaseRepository<WaterDeficiency> repository,
        IChangedModelLogService logService)
        : base(repository)
    {
        _logService = logService;
    }

    public override async Task<WaterDeficiency> UpdateAsync(WaterDeficiency entity)
    {
        WaterDeficiency oldEntity = await base.GetByIdAsync(entity.Id);

        await _logService.LogDeficiencyChangesAsync(oldEntity, entity, EDeficiencyType.Water, entity.CreatedBy);

        return await base.UpdateAsync(entity);
    }

}
