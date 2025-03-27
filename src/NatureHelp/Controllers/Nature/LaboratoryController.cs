using Application.Interfaces.Services.Nature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Authorize(Roles = "SuperAdmin, Owner, Manager, Researcher")]
[Route("api/[controller]")]
public class LaboratoryController : Controller
{
    private readonly ILaboratoryService _laboratoryService;

    public LaboratoryController(ILaboratoryService laboratoryService)
    {
        _laboratoryService = laboratoryService;
    }

    /// <summary>
    /// Get list of laboratories
    /// </summary>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<IActionResult> LabsList([FromQuery] int scrollCount)
    {
        return Ok(await _laboratoryService.GetLabsAsync(scrollCount));
    }

    /// <summary>
    /// Get laboratory by id
    /// </summary>
    /// param name="labId"
    /// <returns></returns>
    [HttpGet("{labId}")]
    public async Task<IActionResult> LabById([FromRoute] Guid labId)
    {
        return Ok(await _laboratoryService.GetLabByIdAsync(labId));
    }

    /// <summary>
    /// Get list of researches
    /// </summary>
    /// <returns></returns>
    [HttpGet("researches")]
    public async Task<IActionResult> ResearchesList([FromQuery] int scrollCount)
    {
        return Ok(await _laboratoryService.GetResearchesList(scrollCount));
    }
}
