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
    [HttpGet]
    public async Task<IActionResult> LabsList()
    {
        return Ok(await _laboratoryService.GetLabsAsync());
    }
}
