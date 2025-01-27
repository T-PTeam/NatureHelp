﻿using Application.Interfaces.Services.Nature;
using Domain.Interfaces;
using Domain.Models.Nature;

namespace Application.Services.Nature;
public class DeficiencyService : IDeficiencyService
{
    private readonly IBaseRepository<WaterDeficiency> _waterRepository;
    private readonly IBaseRepository<SoilDeficiency> _soilRepository;

    public DeficiencyService(IBaseRepository<WaterDeficiency> waterRepository, IBaseRepository<SoilDeficiency> soilRepository)
    {
        _waterRepository = waterRepository;
        _soilRepository = soilRepository;
    }
    public async Task<IEnumerable<WaterDeficiency>> GetWaterDeficiencyList() => await _waterRepository.GetAllAsync();

    public async Task<IEnumerable<SoilDeficiency>> GetSoilDeficiencyList() => await _soilRepository.GetAllAsync();

    public async Task<WaterDeficiency> GetWaterDeficiency(Guid id)
    {
        var deficiency = await _waterRepository.GetByIdAsync(id);

        return deficiency;
    }

    public async Task<SoilDeficiency> GetSoilDeficiency(Guid id)
    {
        var deficiency = await _soilRepository.GetByIdAsync(id);

        return deficiency;
    }

    public async Task<Deficiency> Create(Deficiency deficiency)
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
            throw new InvalidCastException("Can not casst deficiency for correct creating...");
        }
    }

    public async Task<Deficiency> Update(Deficiency deficiency)
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
