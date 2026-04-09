using Domain.Interfaces;
using Domain.Models.Nature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace NatureHelp.Controllers.Nature;

[Route("api/waterdeficiency")]
[AllowAnonymous]
public class WaterDeficiencyController : BaseController<WaterDeficiency>
{
    public readonly IMapObjectsService<WaterDeficiency, DeficiencyMapDto> _mapObjectsService;
    public WaterDeficiencyController(IBaseService<WaterDeficiency> waterDeficiencyService, IMapObjectsService<WaterDeficiency, DeficiencyMapDto> waterMapObjectsDeficiencyService)
        : base(waterDeficiencyService) {
        _mapObjectsService = waterMapObjectsDeficiencyService;
    }

    [HttpGet("map-objects")]
    public virtual async Task<IActionResult> GetDeficienciesForMapAsync([FromQuery] IDictionary<string, string?>? filters) => Ok(await _mapObjectsService.GetMapObjectsAsync(filters));
}