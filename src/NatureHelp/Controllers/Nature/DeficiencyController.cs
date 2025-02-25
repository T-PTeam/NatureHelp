using Application.Interfaces.Services.Nature;
using Domain.Models.Nature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Authorize(Roles = "SuperAdmin, Owner, Manager, Supervisor")]
[Route("api/[controller]")]
public class DeficiencyController : Controller
{
    private readonly IDeficiencyService _deficiencyService;

    public DeficiencyController(IDeficiencyService deficiencyService)
    {
        _deficiencyService = deficiencyService;
    }

    /// <summary>
    /// Get list of water deficiencies
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("water")]
    public async Task<IActionResult> WaterList()
    {
        return Ok(await _deficiencyService.GetWaterDeficiencyListAsync());
    }

    /// <summary>
    /// Get list of soil deficiencies
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("soil")]
    public async Task<IActionResult> SoilList()
    {
        return Ok(await _deficiencyService.GetSoilDeficiencyListAsync());
    }

    /// <summary>
    /// Get details of water deficiency
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("water/{id}")]
    public async Task<IActionResult> Water(Guid id)
    {
        return Ok(await _deficiencyService.GetWaterDeficiencyAsync(id));
    }

    /// <summary>
    /// Get details of soil deficiency
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("soil/{id}")]
    public async Task<IActionResult> Soil(Guid id)
    {
        return Ok(await _deficiencyService.GetSoilDeficiencyAsync(id));
    }

    /// <summary>
    /// Create new water deficiency
    /// </summary>
    /// <param name="deficiency"></param>
    /// <returns></returns>
    [HttpPost("water")]
    public async Task<IActionResult> Create(WaterDeficiency deficiency)
    {
        return Ok(await _deficiencyService.CreateAsync(deficiency));
    }

    /// <summary>
    /// Create new soil deficiency
    /// </summary>
    /// <param name="deficiency"></param>
    /// <returns></returns>
    [HttpPost("soil")]
    public async Task<IActionResult> Create(SoilDeficiency deficiency)
    {
        return Ok(await _deficiencyService.CreateAsync(deficiency));
    }

    /// <summary>
    /// Update water deficiency
    /// </summary>
    /// <param name="deficiency"></param>
    /// <returns></returns>
    [HttpPut("water")]
    public async Task<IActionResult> Update(WaterDeficiency deficiency)
    {
        return Ok(await _deficiencyService.UpdateAsync(deficiency));
    }

    /// <summary>
    /// Update soil deficiency
    /// </summary>
    /// <param name="deficiency"></param>
    /// <returns></returns>
    [Authorize("")]
    [HttpPut("soil")]
    public async Task<IActionResult> Update(SoilDeficiency deficiency)
    {
        return Ok(await _deficiencyService.UpdateAsync(deficiency));
    }
}
