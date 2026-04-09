using Application.Interfaces.Services;
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
    private readonly IAuthenticationService _authService;
    private readonly IProfileService _profileService;
    private readonly IAchievementEvaluationService _achievementEvaluation;
    private new readonly IMapObjectsRepository<SoilDeficiency, DeficiencyMapDto> _repository;
    
    public SoilDeficiencyService(
        IMapObjectsRepository<SoilDeficiency, DeficiencyMapDto> repository,
        IChangedModelLogService logService,
        IAuthenticationService authService,
        IProfileService profileService,
        IAchievementEvaluationService achievementEvaluation)
        : base(repository)
    {
        _logService = logService;
        _authService = authService;
        _profileService = profileService;
        _achievementEvaluation = achievementEvaluation;
        _repository = repository;
    }

    public override async Task<SoilDeficiency> AddAsync(SoilDeficiency entity)
    {
        var created = await base.AddAsync(entity);
        if (entity.CreatedBy != Guid.Empty)
            await _achievementEvaluation.EvaluateForUserAsync(entity.CreatedBy);
        return created;
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

    public override async Task<ListData<SoilDeficiency>> GetList(int scrollCount, IDictionary<string, string?>? filters)
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        var data = await _repository.GetAllAsync(scrollCount, filters, currentUser);

        return new ListData<SoilDeficiency>
        {
            List = data,
            TotalCount = data.Count()
        };
    }


    public override async Task<SoilDeficiency> UpdateAsync(SoilDeficiency entity)
    {
        SoilDeficiency oldEntity = await base.GetByIdAsync(entity.Id);

        if (oldEntity == null) throw new EntityNotFoundException<SoilDeficiency>();

        var logId = await _logService.LogDeficiencyChangesAsync(oldEntity, entity, EDeficiencyType.Soil, entity.CreatedBy);

        var updated = await base.UpdateAsync(entity);

        var editor = await _authService.GetCurrentUserAsync();
        if (editor != null && logId.HasValue)
            await _profileService.TryAwardDeficiencyEditedAsync(editor.Id, logId.Value);

        return updated;
    }

}
