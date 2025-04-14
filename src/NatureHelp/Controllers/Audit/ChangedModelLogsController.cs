using Application.Interfaces.Services.Audit;
using Domain.Enums;
using Domain.Models.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Audit;

[AllowAnonymous]
[Route("api/[controller]")]
public class ChangedModelLogsController : BaseController<ChangedModelLog>
{
    private readonly IChangedModelLogService _changedModelLogService;
    public ChangedModelLogsController(IChangedModelLogService changedModelLogService)
        : base(changedModelLogService)
    {
        _changedModelLogService = changedModelLogService;
    }

    [HttpGet("deficiency-history")]
    public async Task<IActionResult> GetDeficiencyHistoryById([FromQuery] Guid deficiencyId, [FromQuery] EDeficiencyType type)
    {
        return Ok(await _changedModelLogService.GetChangingHistoryByDeficiencyIdAsync(deficiencyId, type));
    }
}
