using Application.Interfaces.Services.Audit;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models.Nature;
using Shared.Dtos;

namespace Application.Services.Nature;
public class SoilDeficiencyService : BaseService<SoilDeficiency>, IMapObjectsService<SoilDeficiency, DeficiencyMapDto>
{
    private readonly IChangedModelLogService _logService;
    private new readonly IMapObjectsRepository<SoilDeficiency, DeficiencyMapDto> _repository;
    public SoilDeficiencyService(
        IMapObjectsRepository<SoilDeficiency, DeficiencyMapDto> repository,
        IChangedModelLogService logService)
        : base(repository)
    {
        _logService = logService;
        _repository = repository;
    }

    public async Task<ListData<DeficiencyMapDto>> GetMapObjectsAsync(IDictionary<string, string?>? filters)
    {
        var data = await _repository.GetMapObjects(filters);

        ListData<DeficiencyMapDto> list = new ListData<DeficiencyMapDto>()
        {
            List = data,
            TotalCount = data.Count()
        };        

        return list;
    }

    public override async Task<SoilDeficiency> UpdateAsync(SoilDeficiency entity)
    {
        SoilDeficiency oldEntity = await base.GetByIdAsync(entity.Id);

        if (oldEntity == null) throw new EntityNotFoundException<SoilDeficiency>();

        await _logService.LogDeficiencyChangesAsync(oldEntity, entity, EDeficiencyType.Water, entity.CreatedBy);

        return await base.UpdateAsync(entity);
    }

}
