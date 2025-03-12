using Domain.Models.Nature;
using Shared.Dtos;

namespace Application.Interfaces.Services.Nature;
public interface IDeficiencyService
{
    public Task<ListData<WaterDeficiency>> GetWaterDeficiencyListAsync(int scrollCount);
    public Task<ListData<SoilDeficiency>> GetSoilDeficiencyListAsync(int scrollCount);
    public Task<WaterDeficiency> GetWaterDeficiencyAsync(Guid id);
    public Task<SoilDeficiency> GetSoilDeficiencyAsync(Guid id);
    public Task<Deficiency> CreateAsync(Deficiency deficiency);
    public Task<Deficiency> UpdateAsync(Deficiency deficiency);
}
