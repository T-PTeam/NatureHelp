using Application.Interfaces.Services.Nature;
using Domain.Models.Nature;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Route("api/[controller]")]
public class DeficiencyController : Controller
{
    private readonly IDeficiencyService _deficiencyService;

    public DeficiencyController(IDeficiencyService deficiencyService)
    {
        _deficiencyService = deficiencyService;
    }

    [HttpGet("water")]
    public async Task<IActionResult> WaterList()
    {
        return Ok(await _deficiencyService.GetWaterDeficiencyList());
    }

    [HttpGet("soil")]
    public async Task<IActionResult> SoilList()
    {
        return Ok(await _deficiencyService.GetSoilDeficiencyList());
    }

    [HttpGet("water/{id}")]
    public async Task<IActionResult> Water(Guid id)
    {
        return Ok(await _deficiencyService.GetWaterDeficiency(id));
    }

    [HttpGet("soil/{id}")]
    public async Task<IActionResult> Soil(Guid id)
    {
        return Ok(await _deficiencyService.GetSoilDeficiency(id));
    }

    [HttpPost("water")]
    public async Task<IActionResult> Create(WaterDeficiency deficiency)
    {
        return Ok(await _deficiencyService.Create(deficiency));
    }

    [HttpPost("soil")]
    public async Task<IActionResult> Create(SoilDeficiency deficiency)
    {
        return Ok(await _deficiencyService.Create(deficiency));
    }

    [HttpPut("water")]
    public async Task<IActionResult> Update(WaterDeficiency deficiency)
    {
        return Ok(await _deficiencyService.Update(deficiency));
    }

    [HttpPut("soil")]
    public async Task<IActionResult> Update(SoilDeficiency deficiency)
    {
        return Ok(await _deficiencyService.Update(deficiency));
    }
}
