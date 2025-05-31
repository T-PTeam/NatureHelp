using Application.Interfaces.Services.Audit;
using Domain.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Nature;

namespace Application.Services.Nature;
public class SoilDeficiencyService : BaseService<SoilDeficiency>
{
    private readonly IChangedModelLogService _logService;
    public SoilDeficiencyService(
        IBaseRepository<SoilDeficiency> repository,
        IChangedModelLogService logService)
        : base(repository)
    {
        _logService = logService;
    }

    public override async Task<SoilDeficiency> UpdateAsync(SoilDeficiency entity)
    {
        SoilDeficiency oldEntity = await base.GetByIdAsync(entity.Id);

        if (oldEntity == null) throw new EntityNotFoundException<SoilDeficiency>();

        await _logService.LogDeficiencyChangesAsync(oldEntity, entity, EDeficiencyType.Water, entity.CreatedBy);

        return await base.UpdateAsync(entity);
    }

}
