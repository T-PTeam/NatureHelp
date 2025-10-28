using Application.Interfaces.Services;
using Application.Interfaces.Services.Audit;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Nature;
using Shared.Dtos;

namespace Application.Services.Nature;
public class WaterDeficiencyService : BaseService<WaterDeficiency>, IMapObjectsService<WaterDeficiency, DeficiencyMapDto>
{
    private readonly IChangedModelLogService _logService;
    private readonly IAuthenticationService _authService;
    private new readonly IMapObjectsRepository<WaterDeficiency, DeficiencyMapDto> _repository;

    public WaterDeficiencyService(
        IMapObjectsRepository<WaterDeficiency, DeficiencyMapDto> repository,
        IChangedModelLogService logService,
        IAuthenticationService authService)
        : base(repository)
    {
        _logService = logService;
        _authService = authService;
        _repository = repository;
    }

    public async Task<ListData<DeficiencyMapDto>> GetMapObjectsAsync(IDictionary<string, string?>? filters)
    {
        var user = await _authService.GetCurrentUserAsync();
        var data = await _repository.GetMapObjects(filters, user);

        ListData<DeficiencyMapDto> list = new ListData<DeficiencyMapDto>()
        {
            List = data,
            TotalCount = data.Count()
        };

        return list;
    }

    public override async Task<ListData<WaterDeficiency>> GetList(int scrollCount, IDictionary<string, string?>? filters)
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        var data = await _repository.GetAllAsync(scrollCount, filters, currentUser);

        return new ListData<WaterDeficiency>
        {
            List = data,
            TotalCount = data.Count()
        };
    }


    public override async Task<WaterDeficiency> UpdateAsync(WaterDeficiency entity)
    {
        WaterDeficiency? oldEntity = await GetByIdAsync(entity.Id);

        if (oldEntity == null)
        {
            throw new NullReferenceException("Can not find entity with ID " + entity.Id);
        }
        await _logService.LogDeficiencyChangesAsync(oldEntity, entity, EDeficiencyType.Water, entity.CreatedBy);

        return await base.UpdateAsync(entity);
    }

}
