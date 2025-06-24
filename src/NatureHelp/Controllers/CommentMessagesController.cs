using Application.Interfaces.Services;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Organization;

[Route("api/[controller]")]
public class CommentMessagesController : BaseController<CommentMessage>
{
    private readonly IModelByDeficiencyService<CommentMessage> _deficiencyCommentsService;

    public CommentMessagesController(IModelByDeficiencyService<CommentMessage> deficiencyCommentsService)
        : base(deficiencyCommentsService)
    {
        _deficiencyCommentsService = deficiencyCommentsService;
    }

    [HttpGet("deficiency/{deficiencyId}")]
    public async Task<IActionResult> GetByDeficiencyId([FromRoute] Guid deficiencyId, [FromQuery] EDeficiencyType deficiencyType)
    {
        return Ok(await _deficiencyCommentsService.GetModelsByDeficiencyIdAsync(deficiencyId, deficiencyType));
    }
}
