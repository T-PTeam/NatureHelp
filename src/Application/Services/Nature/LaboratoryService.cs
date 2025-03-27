using Application.Interfaces.Services.Nature;
using Domain.Models.Analitycs;
using Domain.Models.Organization;
using Infrastructure.Interfaces;
using Shared.Dtos;

namespace Application.Services.Nature;
public class LaboratoryService : ILaboratoryService
{
    private readonly ILaboratoryRepository _laboratoryRepository;
    private readonly IResearchRepository _researchRepository;
    public LaboratoryService(ILaboratoryRepository laboratoryRepository,
        IResearchRepository researchRepository)
    {
        _laboratoryRepository = laboratoryRepository;
        _researchRepository = researchRepository;
    }

    public async Task<ListData<Laboratory>> GetLabsAsync(int scrollCount)
{
        var originalData = await _laboratoryRepository.GetAllAsync(scrollCount);

        var totalCount = await _laboratoryRepository.GetTotalCount();

        var result = new ListData<Laboratory>()
        {
            List = originalData,
            TotalCount = totalCount,
        };

        return result;
    }

    public async Task<Laboratory> GetLabByIdAsync(Guid id)
    {
        var lab = await _laboratoryRepository.GetByIdAsync(id);

        return lab;
    }

    public async Task<Laboratory> UpdateAsync(Laboratory lab)
    {
        await _laboratoryRepository.UpdateAsync(lab);

        return lab;
    }

    public async Task<ListData<Research>> GetResearchesList(int scrollCount)
    {
        var originalData = await _researchRepository.GetAllAsync(scrollCount);

        var totalCount = await _researchRepository.GetTotalCount();

        var result = new ListData<Research>()
        {
            List = originalData,
            TotalCount = totalCount,
        };

        return result;
    }
}
