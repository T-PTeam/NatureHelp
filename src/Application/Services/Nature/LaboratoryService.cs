using Application.Interfaces.Services.Nature;
using Domain.Models.Organization;
using Infrastructure.Interfaces;

namespace Application.Services.Nature;
public class LaboratoryService : ILaboratoryService
{
    private readonly ILaboratoryRepository _laboratoryRepository;
    public LaboratoryService(ILaboratoryRepository laboratoryRepository)
    {
        _laboratoryRepository = laboratoryRepository;
    }

    public async Task<IEnumerable<Laboratory>> GetLabsAsync()
    {
        var labs = await _laboratoryRepository.GetAllAsync();

        return labs;
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
}
