using Domain.Models.Analitycs;
using Domain.Models.Organization;
using Shared.Dtos;

namespace Application.Interfaces.Services.Nature;
public interface ILaboratoryService
{
    public Task<ListData<Laboratory>> GetLabsAsync(int scrollCount);
    public Task<Laboratory> GetLabByIdAsync(Guid id);
    public Task<Laboratory> UpdateAsync(Laboratory lab);
    public Task<ListData<Research>> GetResearchesList(int scrollCount);
}
