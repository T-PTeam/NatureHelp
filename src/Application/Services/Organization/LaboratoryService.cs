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
    private new readonly IMapObjectsRepository<Laboratory, LaboratoryMapDto> _repository;
    public LaboratoryService(
        IMapObjectsRepository<Laboratory, LaboratoryMapDto> repository,
        IChangedModelLogService logService)
        : base(repository)
    {
        _logService = logService;
        _repository = repository;
    }

    public async Task<ListData<LaboratoryMapDto>> GetMapObjectsAsync(IDictionary<string, string?>? filters)
    {
        var data = await _repository.GetMapObjects(filters);

        ListData<LaboratoryMapDto> list = new ListData<LaboratoryMapDto>()
        {
            List = data,
            TotalCount = data.Count()
        };        

        return list;
    }

}
