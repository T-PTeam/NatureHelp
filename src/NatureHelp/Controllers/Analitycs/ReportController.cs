using Application.Interfaces.Services.Analitycs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Analytics;

/// <summary>
/// Controller for Reports
/// </summary>
[ApiController]
[Authorize(Roles = "Owner, Manager, Supervisor")]
[Route("api/[controller]")]
public class ReportController : Controller
{
    private readonly IExcelExportService _excelExportService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="excelExportService"></param>
    public ReportController(IExcelExportService excelExportService)
    {
        _excelExportService = excelExportService;
    }

    /// <summary>
    /// Returns Excel file with water deficiencies
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("water")]
    public async Task<FileResult> GetWaterDeficienciesExcelFile()
    {
        return File(await _excelExportService.GenerateWaterDeficienciesTable(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Water Deficiencies");
    }

    /// <summary>
    /// Returns Excel file with soil deficiencies
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("soil")]
    public async Task<FileResult> GetSoilDeficienciesExcelFile()
    {
        return File(await _excelExportService.GenerateSoilDeficienciesTable(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Water Deficiencies");
    }
}
