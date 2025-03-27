using Application.Interfaces.Services.Nature;
using Domain.Models.Nature;
using Infrastructure.Interfaces;
using Shared.Dtos;

namespace Application.Services.Nature;
public class DeficiencyService : IDeficiencyService
{
    private readonly IDeficiencyRepository<WaterDeficiency> _waterRepository;
    private readonly IDeficiencyRepository<SoilDeficiency> _soilRepository;

    public DeficiencyService(IDeficiencyRepository<WaterDeficiency> waterRepository, IDeficiencyRepository<SoilDeficiency> soilRepository)
    {
        _waterRepository = waterRepository;
        _soilRepository = soilRepository;
    }

    public async Task<ListData<WaterDeficiency>> GetWaterDeficiencyListAsync(int scrollCount)
    {
        var originalData = await _waterRepository.GetAllAsync(scrollCount);

        var totalCount = await _waterRepository.GetTotalCount();

        var result = new ListData<WaterDeficiency>()
        {
            List = originalData,
            TotalCount = totalCount,
        };

        return result;
    }

    public async Task<ListData<SoilDeficiency>> GetSoilDeficiencyListAsync(int scrollCount)
    {
        var originalData = await _soilRepository.GetAllAsync(scrollCount);

        var totalCount = await _soilRepository.GetTotalCount();

        var result = new ListData<SoilDeficiency>()
        {
            List = originalData,
            TotalCount = totalCount,
        };

        return result;
    }

    public async Task<WaterDeficiency> GetWaterDeficiencyAsync(Guid id)
    {
        var deficiency = await _waterRepository.GetByIdAsync(id);

        return deficiency;
    }

    public async Task<SoilDeficiency> GetSoilDeficiencyAsync(Guid id)
    {
        var deficiency = await _soilRepository.GetByIdAsync(id);

        return deficiency;
    }

    public async Task<Deficiency> CreateAsync(Deficiency deficiency)
    {
        if (deficiency is WaterDeficiency waterDeficiency)
        {
            return await _waterRepository.AddAsync(waterDeficiency);
        }
        else if (deficiency is SoilDeficiency soilDeficiency)
        {
            return await _soilRepository.AddAsync(soilDeficiency);
        }
        else
        {
            throw new InvalidCastException("Can not cast deficiency for correct creating...");
        }
    }

    public async Task<Deficiency> UpdateAsync(Deficiency deficiency)
    {
        if (deficiency is WaterDeficiency waterDeficiency)
        {
            return await _waterRepository.UpdateAsync(waterDeficiency);
        }
        else if (deficiency is SoilDeficiency soilDeficiency)
        {
            return await _soilRepository.UpdateAsync(soilDeficiency);
        }
        else
        {
            throw new InvalidCastException("Can not casst deficiency for correct creating...");
        }
    }
}
