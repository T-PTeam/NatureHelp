using Domain.Models.Nature;

namespace Application.Interfaces.Services.Nature;
public interface IDeficiencyService
{
    public Task<IEnumerable<WaterDeficiency>> GetWaterDeficiencyListAsync();
    public Task<IEnumerable<SoilDeficiency>> GetSoilDeficiencyListAsync();
    public Task<WaterDeficiency> GetWaterDeficiencyAsync(Guid id);
    public Task<SoilDeficiency> GetSoilDeficiencyAsync(Guid id);
    public Task<Deficiency> CreateAsync(Deficiency deficiency);
    public Task<Deficiency> UpdateAsync(Deficiency deficiency);
}
