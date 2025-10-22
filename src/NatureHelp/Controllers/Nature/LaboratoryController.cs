using Domain.Interfaces;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Authorize(Roles = "SuperAdmin, Owner, Manager, Researcher")]
[Route("api/[controller]")]
public class LaboratoryController : BaseCachedController<Laboratory>
{
    public readonly IMapObjectsService<Laboratory, LaboratoryMapDto> _mapObjectsService;
    public LaboratoryController(IBaseService<Laboratory> laboratoryService, IMapObjectsService<Laboratory, LaboratoryMapDto> soilMapObjectsDeficiencyService)
        : base(laboratoryService)
    {
        _mapObjectsService = soilMapObjectsDeficiencyService;
    }

    [HttpGet("map-objects")]
    public virtual async Task<IActionResult> GetDeficienciesForMapAsync([FromQuery] IDictionary<string, string?>? filters) => Ok(await _mapObjectsService.GetMapObjectsAsync(filters));
}
