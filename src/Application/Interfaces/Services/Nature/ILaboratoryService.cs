using Domain.Models.Organization;

namespace Application.Interfaces.Services.Nature;
public interface ILaboratoryService
{
    public Task<IEnumerable<Laboratory>> GetLabsAsync();
    public Task<Laboratory> UpdateAsync(Laboratory lab);
}
