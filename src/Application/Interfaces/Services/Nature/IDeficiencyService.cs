using Domain.Models.Nature;

namespace Application.Interfaces.Services.Nature;
public interface IDeficiencyService
{
    public Task<IEnumerable<WaterDeficiency>> GetWaterDeficiencyList();
    public Task<IEnumerable<SoilDeficiency>> GetSoilDeficiencyList();
    public Task<WaterDeficiency> GetWaterDeficiency(Guid id);
    public Task<SoilDeficiency> GetSoilDeficiency(Guid id);
    public Task<Deficiency> Create(Deficiency deficiency);
    public Task<Deficiency> Update(Deficiency deficiency);
}
