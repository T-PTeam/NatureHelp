using Domain.Interfaces;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Organization;

namespace NatureHelp.Controllers.Nature;

[Authorize(Roles = "SuperAdmin, Owner, Manager, Researcher")]
[Route("api/laboratory")]
public class LaboratoryController : BaseCachedController<Laboratory>
{
    private readonly LaboratoryService _laboratoryService;
    public readonly IMapObjectsService<Laboratory, LaboratoryMapDto> _mapObjectsService;
    public LaboratoryController(LaboratoryService laboratoryService, IMapObjectsService<Laboratory, LaboratoryMapDto> soilMapObjectsDeficiencyService)
        : base(laboratoryService)
    {
        _laboratoryService = laboratoryService;
        _mapObjectsService = soilMapObjectsDeficiencyService;
    }

    [AllowAnonymous]
    [HttpGet("")]
    public override async Task<IActionResult> GetList([FromQuery] int scrollCount, [FromQuery] IDictionary<string, string?>? filters)
        => await base.GetList(scrollCount, filters);

    [AllowAnonymous]
    [HttpGet("map-objects")]
    public virtual async Task<IActionResult> GetDeficienciesForMapAsync([FromQuery] IDictionary<string, string?>? filters) => Ok(await _mapObjectsService.GetMapObjectsAsync(filters));

    [HttpPut("{id}/researchers")]
    public async Task<IActionResult> AssignResearchers(Guid id, [FromBody] IEnumerable<Guid> researcherIds)
    {
        await _laboratoryService.AssignResearchersAsync(id, researcherIds);
        return Ok();
    }
}
