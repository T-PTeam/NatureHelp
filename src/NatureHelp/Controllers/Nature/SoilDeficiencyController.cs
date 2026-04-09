using Domain.Interfaces;
using Domain.Models.Nature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace NatureHelp.Controllers.Nature;

[Route("api/soildeficiency")]
[AllowAnonymous]
public class SoilDeficiencyController : BaseController<SoilDeficiency>
{
    public readonly IMapObjectsService<SoilDeficiency, DeficiencyMapDto> _mapObjectsService;
    public SoilDeficiencyController(IBaseService<SoilDeficiency> soilDeficiencyService, IMapObjectsService<SoilDeficiency, DeficiencyMapDto> soilMapObjectsDeficiencyService)
        : base(soilDeficiencyService) {
        _mapObjectsService = soilMapObjectsDeficiencyService;
    }

    [HttpGet("map-objects")]
    public virtual async Task<IActionResult> GetDeficienciesForMapAsync([FromQuery] IDictionary<string, string?>? filters) => Ok(await _mapObjectsService.GetMapObjectsAsync(filters));
}