using Domain.Models.Nature;

namespace Application.Interfaces.Services.Nature;
public interface IDeficiencyService
{
    public Task<IEnumerable<WaterDeficiency>> GetWaterDeficiencies();
    public Task<IEnumerable<SoilDeficiency>> GetSoilDeficiencies();
    public Task<Deficiency> Create(Deficiency deficiency);
    public Task<Deficiency> Update(Deficiency deficiency);
}
