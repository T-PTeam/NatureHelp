using Application.Interfaces.Services;
using Application.Interfaces.Services.Audit;
using Domain.Enums;
using Domain.Models.Nature;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Organization;

[Route("api/[controller]")]
public class AttachmentsController : BaseController<DeficiencyAttachment>
{
    private readonly IModelByDeficiencyService<DeficiencyAttachment> _deficiencyAttachmentService;
    private readonly IBlobStorageProvider _blobStorageProvider;
    private readonly IChangedModelLogService _logService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IProfileService _profileService;

    public AttachmentsController(
        IModelByDeficiencyService<DeficiencyAttachment> deficiencyAttachmentService,
        IBlobStorageProvider blobStorageProvider,
        IChangedModelLogService logService,
        IAuthenticationService authenticationService,
        IProfileService profileService)
        : base(deficiencyAttachmentService)
    {
        _deficiencyAttachmentService = deficiencyAttachmentService;
        _blobStorageProvider = blobStorageProvider;
        _logService = logService;
        _authenticationService = authenticationService;
        _profileService = profileService;
    }

    [HttpGet("deficiency/{deficiencyId}")]
    public async Task<IActionResult> GetByDeficiencyId([FromRoute] Guid deficiencyId, [FromQuery] EDeficiencyType deficiencyType)
    {
        return Ok(await _deficiencyAttachmentService.GetModelsByDeficiencyIdAsync(deficiencyId, deficiencyType));
    }

    [HttpPost("upload/deficiency/{deficiencyId}")]
    public async Task<IActionResult> UploadAttachment(
        [FromForm] IFormFile file,
        [FromRoute] Guid deficiencyId,
        [FromQuery] int deficiencyType)
    {
        if (file is null || file.Length == 0)
            return BadRequest("File is missing");

        using var stream = file.OpenReadStream();
        var url = await _blobStorageProvider.UploadFileAsync(stream, file.FileName, file.ContentType);

        var uploader = await _authenticationService.GetCurrentUserAsync();

        var attachment = new DeficiencyAttachment
        {
            Id = Guid.NewGuid(),
            DeficiencyId = deficiencyId,
            FileName = file.FileName,
            FileSize = file.Length,
            MimeType = file.ContentType,
            StoragePath = url,
            PreviewUrl = url,
            DeficiencyType = (EDeficiencyType)deficiencyType,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = uploader?.Id ?? Guid.Empty
        };

        if (!string.IsNullOrEmpty(url))
        {
            await _logService.LogDeficiencyAttachmentChangeAsync<Deficiency>(deficiencyId, attachment.FileName, attachment.FileSize, attachment.PreviewUrl, (EDeficiencyType)deficiencyType);
        }

        var saved = await _deficiencyAttachmentService.AddAsync(attachment);

        if (uploader != null)
            await _profileService.TryAwardPhotoAddedAsync(uploader.Id, saved.Id);

        return Ok(new { previewUrl = url });
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadAttachment([FromRoute] Guid id)
    {
        var attachment = await _deficiencyAttachmentService.GetByIdAsync(id);
        if (attachment is null) return NotFound();

        var stream = await _blobStorageProvider.GetFileStreamAsync(attachment.StoragePath);
        return File(stream, attachment.MimeType, attachment.FileName);
    }

}
