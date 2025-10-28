using Application.Interfaces.Services;
using Application.Interfaces.Services.Audit;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models.Nature;
using Domain.Models.Organization;
using Shared.Dtos;

namespace Application.Services.Organization;
public class LaboratoryService : BaseService<Laboratory>, IMapObjectsService<Laboratory, LaboratoryMapDto>
{
    private readonly IChangedModelLogService _logService;
    private readonly IAuthenticationService _authService;
    private new readonly IMapObjectsRepository<Laboratory, LaboratoryMapDto> _repository;
    public LaboratoryService(
        IMapObjectsRepository<Laboratory, LaboratoryMapDto> repository,
        IChangedModelLogService logService,
        IAuthenticationService authService)
        : base(repository)
    {
        _logService = logService;
        _authService = authService;
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
}
