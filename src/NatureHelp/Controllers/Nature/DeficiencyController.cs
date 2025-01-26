using Application.Interfaces.Services.Nature;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;
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
        return Ok(await _deficiencyService.GetWaterDeficiencies());
    }

    [HttpGet("soil")]
    public async Task<IActionResult> SoilList()
    {
        return Ok(await _deficiencyService.GetSoilDeficiencies());
    }
}
