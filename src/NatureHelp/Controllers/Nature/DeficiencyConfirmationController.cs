using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[ApiController]
[Authorize(Roles = "SuperAdmin,Owner,Manager,Supervisor,Researcher")]
[Route("api/deficiency")]
public class DeficiencyConfirmationController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IAuthenticationService _authenticationService;

    public DeficiencyConfirmationController(
        IProfileService profileService,
        IAuthenticationService authenticationService)
    {
        _profileService = profileService;
        _authenticationService = authenticationService;
    }

    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm([FromRoute] Guid id, [FromQuery] int deficiencyType, CancellationToken ct)
    {
        var user = await _authenticationService.GetCurrentUserAsync();
        if (user == null)
            return Unauthorized();

        if (!Enum.IsDefined(typeof(EDeficiencyType), deficiencyType))
            return BadRequest("Invalid deficiency type");

        var result = await _profileService.ConfirmDeficiencyAsync(user.Id, id, (EDeficiencyType)deficiencyType, ct);
        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new
        {
            alreadyConfirmed = result.AlreadyConfirmed,
            creatorRewarded = result.CreatorRewarded,
        });
    }
}
