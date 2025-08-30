using Application.Interfaces.Services.Organization;
using Domain.Enums;
using Domain.Models.Audit;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace NatureHelp.Controllers.Audit;

[Route("api/[controller]")]
public class MonitoringController : BaseController<DeficiencyMonitoring>
{
    private readonly MonitoringService _monitoringService;
    private readonly IUserService _userService;
    public MonitoringController(MonitoringService monitoringService,
        IUserService userService)
        : base(monitoringService)
    {
        _monitoringService = monitoringService;
        _userService = userService;
    }

    [HttpPut("toggle-monitoring")]
    public async Task<IActionResult> ToggleMonitoring([FromQuery] Guid? deficiencyId, [FromQuery] EDeficiencyType type, [FromBody] TokensDto tokensDto)
    {
        return Ok(await _monitoringService.ToggleMonitoringAsync(tokensDto.RefreshToken ?? "", deficiencyId, type));
    }
}
