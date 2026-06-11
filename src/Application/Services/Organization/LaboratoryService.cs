using Application.Interfaces.Services;
using Application.Interfaces.Services.Audit;
using Application.Interfaces.Services.Cache;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Organization;
using Shared.Dtos;

namespace Application.Services.Organization;
public class LaboratoryService : BaseService<Laboratory>, IMapObjectsService<Laboratory, LaboratoryMapDto>
{
    private readonly IChangedModelLogService _logService;
    private readonly IAuthenticationService _authService;
    private readonly IRedisCacheService? _redisCacheService;
    private new readonly IMapObjectsRepository<Laboratory, LaboratoryMapDto> _repository;

    public LaboratoryService(
        IMapObjectsRepository<Laboratory, LaboratoryMapDto> repository,
        IChangedModelLogService logService,
        IAuthenticationService authService,
        IRedisCacheService redisCacheService)
        : base(repository, redisCacheService)
    {
        _logService = logService;
        _authService = authService;
        _redisCacheService = redisCacheService;
        _repository = repository;
    }

    public async Task<ListData<LaboratoryMapDto>> GetMapObjectsAsync(IDictionary<string, string?>? filters)
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        var data = await _repository.GetMapObjects(filters, currentUser);

        ListData<LaboratoryMapDto> list = new ListData<LaboratoryMapDto>()
        {
            List = data,
            TotalCount = data.Count()
        };

        return list;
    }

    public override async Task<ListData<Laboratory>> GetList(int scrollCount, IDictionary<string, string?>? filters)
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        var data = await _repository.GetAllAsync(scrollCount, filters, currentUser);

        return new ListData<Laboratory>
        {
            List = data,
            TotalCount = data.Count()
        };
    }

    public override async Task<Laboratory> AddAsync(Laboratory entity)
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        if (currentUser != null)
        {
            entity.CreatedBy = currentUser.Id;
        }

        if (entity.CreatedOn == default)
        {
            entity.CreatedOn = DateTime.UtcNow;
        }

        var result = await base.AddAsync(entity);
        await InvalidateListCacheAsync();
        return result;
    }

    public override async Task<Laboratory> UpdateAsync(Laboratory entity)
    {
        var currentUser = await _authService.GetCurrentUserAsync()
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var existing = await _repository.GetByIdAsync(entity.Id)
            ?? throw new KeyNotFoundException("Laboratory was not found.");

        if (!CanEditLaboratory(existing, currentUser))
        {
            throw new UnauthorizedAccessException("You can only edit laboratories that you created or belong to.");
        }

        var result = await base.UpdateAsync(entity);
        await InvalidateListCacheAsync();
        return result;
    }

    private static bool CanEditLaboratory(Laboratory laboratory, User currentUser)
    {
        if (currentUser.Role == ERole.SuperAdmin)
        {
            return true;
        }

        if (laboratory.CreatedBy == currentUser.Id)
        {
            return true;
        }

        return currentUser.LaboratoryId == laboratory.Id;
    }

    public override async Task<Guid> DeleteAsync(Guid id)
    {
        var result = await base.DeleteAsync(id);
        await InvalidateListCacheAsync();
        return result;
    }

    private Task InvalidateListCacheAsync()
    {
        if (_redisCacheService == null)
        {
            return Task.CompletedTask;
        }

        return _redisCacheService.RemoveAsync(typeof(Laboratory).Name + "_listdata");
    }
}
